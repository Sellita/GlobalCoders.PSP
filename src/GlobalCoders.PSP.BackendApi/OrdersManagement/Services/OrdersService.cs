using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Enums;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Services;
using GlobalCoders.PSP.BackendApi.InventoryManagement.Services;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Helpers;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Repositories;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Services;
using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;
using GlobalCoders.PSP.BackendApi.TaxManagement.Factories;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.TaxManagement.Services;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Services;

public class OrdersService : IOrdersService
{
    private readonly ILogger<OrdersService> _logger;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IProductService _productService;
    private readonly IEmployeeService _employeeService;
    private readonly ITaxService _taxService;
    private readonly IInventoryService _inventoryService;

    private const string FailedToChangeStatusMessage = "Failed to change status";

    private readonly Dictionary<OrderStatus, Func<OrderEntity, Task<(bool, string)>>>
        _statusChangeMethods;

    public OrdersService(
        ILogger<OrdersService> logger,
        IOrdersRepository ordersRepository, 
        IProductService productService,
        IEmployeeService employeeService,
        ITaxService taxService,
        IInventoryService inventoryService)
    {
        _statusChangeMethods = new Dictionary<OrderStatus, Func<OrderEntity, Task<(bool, string)>>>
        {
            { OrderStatus.Open, ChangeToOpen },
            { OrderStatus.Closed, ChangeToClosed },
            { OrderStatus.Paid, ChangeToPaid },
            { OrderStatus.Cancelled, ChangeToCanceled },
            { OrderStatus.Refunded, ChangeToRefunded }
        };
        _logger = logger;
        _ordersRepository = ordersRepository;
        _productService = productService;
        _employeeService = employeeService;
        _taxService = taxService;
        _inventoryService = inventoryService;
    }
    public async Task<(bool, string)> UpdateAsync(OrderEntity updateModel)
    {
        var order = await _ordersRepository.GetAsync(updateModel.Id);
        
        if (order == null)
        {
            return (false, "order not found");
        }
        
        if(order.Status != OrderStatus.Open)
        {
            return (false, "Order can be updated only in open status");
        }
        
        updateModel.Status = order.Status;
        
        return (await _ordersRepository.UpdateAsync(updateModel), "Failed to update order");
    }

    public async Task<bool> CreateAsync(OrderEntity createModel, CancellationToken cancellationToken)
    {
        var employee =  (await _employeeService.GetAsync(createModel.EmployeeId, cancellationToken));
       
        if(employee == null)
        {
            return false;
        }
        
        if(employee.MerchantId != createModel.MerchantId)
        {
            return false;
        }
        
        return await _ordersRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<OrdersListModel>> GetAllAsync(OrdersFilter filter)
    {
        var entities = await _ordersRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(OrderListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<OrderResponseModel?> GetAsync(Guid orderId)
    {
        var entity = await _ordersRepository.GetAsync(orderId);

        if (entity == null)
        {
            return null;
        }
        
        return OrderResponseModelFactory.Create(entity);
    }

    public Task<bool> DeleteAsync(Guid organizationId)
    {
        return _ordersRepository.DeleteAsync(organizationId);
    }

    public async Task<bool> HasPermissionAsync(Guid orderId, Guid? userMerchantId)
    {
        return await _ordersRepository.OrderBelongsToMerchantAsync(orderId, userMerchantId);
    }

    public async Task<(bool result, string message)> ChangeStatusAsync(OrderChangeStatusRequestModel orderChangeStatusRequest)
    {
        var order = await _ordersRepository.GetAsync(orderChangeStatusRequest.OrderId);
        
        if(order == null)
        {
            return (false, "Order not found");
        }

        if (!_statusChangeMethods.TryGetValue(orderChangeStatusRequest.NewStatus, out var method))
        {
            return (false, "Invalid status change");
        }
        
        return await method(order);
    }
    
    private async Task<(bool, string)> ChangeToRefunded(OrderEntity order)
    {
        if(order.Status != OrderStatus.Paid && order.Status != OrderStatus.Closed)
        {
            return (false, "Refunded status can be set only for paid orders");
        }
        
        foreach (var product in order.OrderProducts)
        {
            await _inventoryService.ChangeQuantityAsync(order.MerchantId, product.ProductId, product.Quantity, CancellationToken.None);
        }
        
        order.Status = OrderStatus.Refunded;
        
        return (await _ordersRepository.UpdateAsync(order), FailedToChangeStatusMessage);
    }

    private async Task<(bool, string)> ChangeToCanceled(OrderEntity order)
    {
        if(order.Status != OrderStatus.Open)
        {
            return (false, "Cancelled status can be set only for open orders");
        }
       
        order.Status = OrderStatus.Cancelled;
        
        return (await _ordersRepository.UpdateAsync(order), FailedToChangeStatusMessage);
    }

    private async Task<(bool, string)> ChangeToPaid(OrderEntity order)
    {
        if(order.Status != OrderStatus.Closed)
        {
            return (false, "Paid status can be set only for closed orders");
        }
        
        if(CalculationHelpers.CalculateTotalPrice(order) != order.OrderPayments.Sum(x=>x.Amount))
        {
            return (false, "Order not fully paid");
        }
        
        order.Status = OrderStatus.Paid;
        
        return (await _ordersRepository.UpdateAsync(order), FailedToChangeStatusMessage);
    }

    private async Task<(bool, string)> ChangeToClosed(OrderEntity order)
    {
        var (validationResult, message) = await ValidateStatusChangeToClosed(order);
        
        if(!validationResult)
        {
            return (validationResult, message);
        }

        order.Status = OrderStatus.Closed;
        
        var taxes = await _taxService.GetAllAsync(TaxFilterFactory.CreateForAllItems(order.MerchantId));
        var discounts = order.OrderDiscounts
            .Where(x=>x.Discount != null)
            .Select(x=>x.Discount!)
            .ToList(); 
        
        foreach (var orderProduct in order.OrderProducts)
        {
            if(orderProduct.Product == null)
            {
                return (false, "Product not found");
            }
            
            await _inventoryService.ChangeQuantityAsync(order.MerchantId, orderProduct.ProductId, -orderProduct.Quantity, CancellationToken.None);
            
            orderProduct.Price = CalculationHelpers.RoundToTwoDecimalPlaces(orderProduct.Product.Price);
            orderProduct.OrderProductTaxes = CalculateTaxes(taxes.Items, orderProduct);
            orderProduct.Discount = CalculateDiscount(discounts, orderProduct);
            orderProduct.ProductName = orderProduct.Product.DisplayName;
        }
        
        order.TotalPrice = CalculationHelpers.CalculatePriceWithTax(order);

        order.Discount = CalculateOrderDiscount(discounts, order);
        order.TotalPriceWithDiscount = CalculationHelpers.CalculateTotalPrice(order);
        
        
        return (await _ordersRepository.UpdateAsync(order), FailedToChangeStatusMessage);
    }

 

    private async Task<(bool, string)> ValidateStatusChangeToClosed(OrderEntity order)
    {
        if(order.Status != OrderStatus.Open)
        {
            return (false, "Closed status can be set only for open orders");
        }
        
        if(order.OrderProducts.Count == 0)
        {
            return (false, "Order has no products");
        }

        foreach (var product in order.OrderProducts)
        {
            var quantity = await _inventoryService.GetQuantityAsync(order.MerchantId, product.ProductId);
            if(quantity < product.Quantity)
            {
                return (false, "Not enough products in inventory");
            }
        }

        return (true, string.Empty);
    }

    private decimal CalculateOrderDiscount(List<DiscountEntity> discountsItems, OrderEntity order)
    {
        var currentTime = DateTime.UtcNow;
        var discountToApply = discountsItems.Where(x =>
            x.ProductTypeId == null
            && x.ProductId == null
            && DiscountTimeValid(x, currentTime)).ToList();

        _logger.LogInformation("Applying discounts {@Discounts} for order {OrderId}", discountToApply, order.Id);
        var calculatedDiscount = 0m;
        foreach (var discount in discountToApply)
        {
            if (discount.Type == DiscountType.Percentage)
            {
                calculatedDiscount += order.TotalPrice * discount.Value / 100;
            }
            else
            {
                calculatedDiscount += discount.Value;
            }
        }
        _logger.LogInformation("CalculatedDiscount: {CalculateDiscount}", calculatedDiscount);

        return CalculationHelpers.RoundToTwoDecimalPlaces( calculatedDiscount);
    }

    private static decimal CalculateDiscount(List<DiscountEntity> discountsItems, OrderProductEntity product)
    {
        var currentTime = DateTime.UtcNow;
        var productDiscounts = discountsItems.Where(x =>
           ( x.ProductTypeId == product.Product?.ProductTypeId
            || x.ProductId == product.ProductId)
            && DiscountTimeValid(x, currentTime)).ToList();
       
        var totalDiscount = 0m;
        foreach (var discount in productDiscounts)
        {
            if(discount.Type == DiscountType.Percentage)
            {
                totalDiscount += product.Price * discount.Value / 100;
            }
            else
            {
                totalDiscount += discount.Value;
            }
        }

        return totalDiscount;
    }

    private static bool DiscountTimeValid(DiscountEntity x, DateTime currentTime)
    {
        return (
            (x.StartDate <= currentTime && x.EndDate >= currentTime)
            || (x.StartDate == null && x.EndDate == null)
        );
    }

    public static ICollection<OrderProductTaxEntity> CalculateTaxes(List<TaxListModel> taxes, OrderProductEntity product)
    {
        var productTaxes = taxes.Where(x =>
            x.ProductTypeId == product.Product?.ProductTypeId
            || x.ProductTypeId == null).ToList();
       
        var calculateTaxes = new List<OrderProductTaxEntity>();
        
        foreach (var tax in productTaxes)
        {
            var taxValue = tax.Value;
            if(tax.Type == TaxType.Percentage)
            {
                taxValue = CalculationHelpers.RoundToTwoDecimalPlaces(product.Price * tax.Value / 100);
            }
            
            calculateTaxes.Add(OrderProductTaxEntityFactory.Create(tax, taxValue, product.Id));
        }

        return calculateTaxes;
    }

    private static Task<(bool, string)> ChangeToOpen(OrderEntity order)
    {
        //initial status, cant be set up manually
        return Task.FromResult((false, "Open status can't be set manually"));
    }

    public async Task<(bool result, string message)> ChangeOrderProductAsync(
        OrderChangeProductRequestModel orderChangeStatusRequest, CancellationToken cancellationToken)
    {
        var order = await _ordersRepository.GetAsync(orderChangeStatusRequest.OrderId);

        if (order == null)
        {
            return (false, "Order not found");
        }

        if(order.Status != OrderStatus.Open)
        {
            return (false, "Products can be changed only for open orders");
        }
        
        var productFromList = order.OrderProducts.FirstOrDefault(x => x.ProductId == orderChangeStatusRequest.ProductId);
        
        if(productFromList != null)
        {
            return await UpdateExistingProduct(orderChangeStatusRequest, productFromList, order);
        }
        
        var product = await _productService.GetAsync(orderChangeStatusRequest.ProductId);
        
        if(product == null)
        {
            return (false, "Product not found");
        }
        
        if(product.MerchantId != order.MerchantId)
        {
            return (false, "Product not belongs to order merchant");
        }
        
        order.OrderProducts.Add(OrderProductEntityFactory.Create(product, orderChangeStatusRequest.Quantity));
        
        return (await _ordersRepository.UpdateAsync(order), "Failed to add product to order");
    }

    private async Task<(bool result, string message)> UpdateExistingProduct(OrderChangeProductRequestModel orderChangeStatusRequest,
        OrderProductEntity productFromList, OrderEntity order)
    {
        productFromList.Quantity += orderChangeStatusRequest.Quantity;
        if(productFromList.Quantity < 0)
        {
            return (false, "Quantity can't be negative");
        }
            
        if(productFromList.Quantity == 0)
        {
            order.OrderProducts.Remove(productFromList);

            return (await _ordersRepository.DeleteProductFromLustAsync(productFromList), "Failed to delete product from list");
        }
            
        return (await _ordersRepository.UpdateAsync(order), "Failed to change product quantity");
    }

    public async Task<(bool result, string message)> MakePaymentAsync(OrderMakePaymentRequestModel orderMakePaymentRequest, CancellationToken cancellationToken)
    {
        var order = await _ordersRepository.GetAsync(orderMakePaymentRequest.OrderId);

        if(order?.Status != OrderStatus.Closed)
        {
            return (false, "Products can be changed only for closed orders");
        }
        
        var leftToPay = CalculationHelpers.CalculateLeftToPay(order);
        
        if(leftToPay < orderMakePaymentRequest.Amount)
        {
            return (false, "Amount is bigger than left to pay");
        }
        
        order.OrderPayments.Add(OrderPaymentsEntityFactory.Create(orderMakePaymentRequest));
        
        if(leftToPay == orderMakePaymentRequest.Amount)
        {
            order.Status = OrderStatus.Paid;
        }
        
        return (await _ordersRepository.UpdateAsync(order), "Failed to make payment");
    }

    public async Task<(bool result, string message)> ChangeTipsAsync(TipsRequestModel tipsRequest, CancellationToken cancellationToken)
    {
        var order = await _ordersRepository.GetAsync(tipsRequest.OrderId);

        if(order?.Status != OrderStatus.Open)
        {
            return (false, "Tips can be changed only for open orders");
        }

        order.Tips = CalculationHelpers.RoundToTwoDecimalPlaces(tipsRequest.Value);
        
        var result = await _ordersRepository.UpdateAsync(order);
        
        return (result, "Failed to change tips");
    }
}