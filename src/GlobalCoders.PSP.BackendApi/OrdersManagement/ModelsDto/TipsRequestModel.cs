namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class TipsRequestModel
{
    public Guid OrderId { get; set; }
    public decimal Value { get; set; }
}