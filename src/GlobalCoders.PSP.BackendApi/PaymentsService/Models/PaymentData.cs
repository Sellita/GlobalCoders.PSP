namespace GlobalCoders.PSP.BackendApi.PaymentsService.Models;

public class PaymentData
{
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string OrderId { get; set; } = string.Empty;
}