using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderDiscountEntityFactory
{
    public static OrderDiscountsEntity Create(OrderDiscountCreateModel orderDiscountCreateModel)
    {
        return new OrderDiscountsEntity
        {
            DiscountId = orderDiscountCreateModel.DiscountId,
        };
    }
}