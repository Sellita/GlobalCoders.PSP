using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderPaymentsEntityFactory
{
    public static OrderPaymentsEntity Create(OrderMakePaymentRequestModel request)
    {
        return new OrderPaymentsEntity
        {
            Amount = request.Amount,
            OrderId = request.OrderId,
            PaymentDate = DateTime.UtcNow,
            Type = request.Type,
        };
    }
}