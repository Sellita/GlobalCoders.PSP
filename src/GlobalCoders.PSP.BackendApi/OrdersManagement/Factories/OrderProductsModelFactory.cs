using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderProductsModelFactory
{
    public static OrderProductsModel Create(OrderProductEntity orderProductEntity)
    {
        return new OrderProductsModel
        {
            Id = orderProductEntity.Id,
            ProductName = orderProductEntity.ProductName,
            Quantity = orderProductEntity.Quantity,
            Price = orderProductEntity.Price,
            Tax = orderProductEntity.OrderProductTaxes.Select(OrderTaxModelFactory.Create).ToArray(),
            Discount = orderProductEntity.Discount
        };
    }
}