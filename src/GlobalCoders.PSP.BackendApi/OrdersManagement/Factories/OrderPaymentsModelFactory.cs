using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderPaymentsModelFactory
{
    public static OrderPaymentsModel Create(OrderPaymentsEntity paymentEntity)
    {
        return new OrderPaymentsModel
        {
            Id = paymentEntity.Id,
            Amount = paymentEntity.Amount
        };
    }
}