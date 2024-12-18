namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderResumePaymentModel
{
    public Guid OrderId { get; set; }
    public Guid PaymentId { get; set; }
}