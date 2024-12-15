using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderTaxModelFactory
{
    public static OrderTaxModel Create(OrderProductTaxEntity entity)
    {
        return new OrderTaxModel
        {
            Name = entity.Name,
            Value = entity.Value
        };
    }
}