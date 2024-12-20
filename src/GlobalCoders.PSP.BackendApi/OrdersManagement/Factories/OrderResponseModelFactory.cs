using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Helpers;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderResponseModelFactory
{
    public static OrderResponseModel Create(OrderEntity orderEntity)
    {
        //todo extract calculations to helper
        return new OrderResponseModel
        {
            Id = orderEntity.Id,
            ClientName = orderEntity.ClientName,
            Tips = orderEntity.Tips,
            TotalTax = orderEntity.OrderProducts.Sum(x => x.OrderProductTaxes.Sum(y=>y.Value) * x.Quantity),
            Price = orderEntity.OrderProducts.Sum(x => x.Price * x.Quantity),
            PriceWithTax = orderEntity.TotalPrice,
            TotalPrice = orderEntity.TotalPriceWithDiscount,
            Discount = orderEntity.Discount,
            PaidSum = orderEntity.OrderPayments.Sum(x => x.Amount),
            Status = orderEntity.Status,
            Products = orderEntity.OrderProducts.Select(OrderProductsModelFactory.Create).ToList(),
            Payments = orderEntity.OrderPayments.Select(OrderPaymentsModelFactory.Create).ToList(),
            Discounts = orderEntity.OrderDiscounts.Select(OrderDiscountModelFactory.Create).ToList(),
            
            EmployeeId = orderEntity.EmployeeId,
            EmployeeName = orderEntity.Employee?.Name ?? string.Empty,
            MerchantId = orderEntity.MerchantId,
            MerchantName = orderEntity.Merchant?.DisplayName?? string.Empty,
            Date = orderEntity.CreatedAt,
        };
    }


}