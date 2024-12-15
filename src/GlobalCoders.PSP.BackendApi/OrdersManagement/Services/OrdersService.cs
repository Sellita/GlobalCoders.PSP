using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Controllers;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Services;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Helpers;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Repositories;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Controllers;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Services;
using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;
using GlobalCoders.PSP.BackendApi.TaxManagement.Factories;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.TaxManagement.Services;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IProductService _productService;
    private readonly IEmployeeService _employeeService;
    private readonly ITaxService _taxService;

    private readonly Dictionary<OrderStatus, Func<OrderEntity, Task<(bool, string)>>>
        StatusChangeMethods;

    public OrdersService(IOrdersRepository ordersRepository, IProductService productService,
        IEmployeeService employeeService,
        ITaxService taxService)
    {
        StatusChangeMethods = new Dictionary<OrderStatus, Func<OrderEntity, Task<(bool, string)>>>
        {
            { OrderStatus.Open, ChangeToOpen },
            { OrderStatus.Closed, ChangeToClosed },
            { OrderStatus.Paid, ChangeToPaid },
            { OrderStatus.Cancelled, ChangeToCanceled },
            { OrderStatus.Refunded, ChangeToRefunded }
        };
        _ordersRepository = ordersRepository;
        _productService = productService;
        _employeeService = employeeService;
        _taxService = taxService;
    }
    public async Task<bool> UpdateAsync(OrderEntity updateModel)
    {
        return await _ordersRepository.UpdateAsync(updateModel);
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

        if (!StatusChangeMethods.TryGetValue(orderChangeStatusRequest.NewStatus, out var method))
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
        
        //todo here should be refund logic
        
        order.Status = OrderStatus.Refunded;
        
        return (await _ordersRepository.UpdateAsync(order), "Failed to change status");
    }

    private async Task<(bool, string)> ChangeToCanceled(OrderEntity order)
    {
        if(order.Status != OrderStatus.Open)
        {
            return (false, "Cancelled status can be set only for open orders");
        }
       
        order.Status = OrderStatus.Cancelled;
        
        return (await _ordersRepository.UpdateAsync(order), "Failed to change status");
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
        
        return (await _ordersRepository.UpdateAsync(order), "Failed to change status");
    }

    private async Task<(bool, string)> ChangeToClosed(OrderEntity order)
    {
        if(order.Status != OrderStatus.Open)
        {
            return (false, "Closed status can be set only for open orders");
        }
        
        if(order.OrderProducts.Count == 0)
        {
            return (false, "Order has no products");
        }
        
        order.Status = OrderStatus.Closed;
        
        var taxes = await _taxService.GetAllAsync(TaxFilterFactory.CreateForAllItems(order.MerchantId));
        
        foreach (var orderProduct in order.OrderProducts)
        {
            orderProduct.Price = CalculationHelpers.RoundToTwoDecimalPlaces(orderProduct.Product.Price);
            orderProduct.OrderProductTaxes = CalculateTaxes(taxes.Items, orderProduct);
            orderProduct.Discount = 0; // todo calculate discount
            orderProduct.ProductName = orderProduct.Product.DisplayName;
        }
        
        return (await _ordersRepository.UpdateAsync(order), "Failed to change status");
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

        if(order.Status != OrderStatus.Open)
        {
            return (false, "Products can be changed only for open orders");
        }
        
        var productFromList = order.OrderProducts.FirstOrDefault(x => x.ProductId == orderChangeStatusRequest.ProductId);
        
        if(productFromList != null)
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
}