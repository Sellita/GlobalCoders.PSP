namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderPaymentsModel
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    
    public bool IsPaid { get; set; }
}