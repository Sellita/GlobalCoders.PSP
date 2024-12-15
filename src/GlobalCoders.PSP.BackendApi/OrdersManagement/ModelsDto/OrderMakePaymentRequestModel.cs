using GlobalCoders.PSP.BackendApi.PaymentsService.Enums;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderMakePaymentRequestModel
{
    public decimal Amount { get; set; }
    public Guid OrderId { get; set; }
    public PaymentType Type { get; set; }
}