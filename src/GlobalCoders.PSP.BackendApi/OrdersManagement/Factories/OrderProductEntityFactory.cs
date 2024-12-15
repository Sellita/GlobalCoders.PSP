using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderProductEntityFactory
{
    public static OrderProductEntity Create(ProductResponseModel productValue, decimal quantity)
    {
        return new OrderProductEntity
        {
            ProductId = productValue.Id,
            ProductName = productValue.DisplayName,
            Quantity = quantity,
            Price = productValue.Price,
            Tax = 0,
            Discount = 0
        };
    }
}