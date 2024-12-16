using GlobalCoders.PSP.BackendApi.DiscountManagement.Enums;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderDiscountModelFactory
{
    public static OrderDiscountModel Create(OrderDiscountsEntity orderProductDiscountEntity)
    {
            return new OrderDiscountModel
            {
                DiscountId = orderProductDiscountEntity.DiscountId,
                Name = orderProductDiscountEntity.Discount?.Name ?? string.Empty,
                Value = orderProductDiscountEntity.Discount?.Value ?? 0,
                Type = orderProductDiscountEntity.Discount?.Type ?? DiscountType.Percentage,
            };
    }
}