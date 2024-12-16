namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderCreateModel
{
    public string ClientName { get; set; } = string.Empty;
    public Guid? EmployeeId { get; set; }
    public Guid? MerchantId { get; set; }

    public List<OrderDiscountCreateModel> Discounts { get; set; } = new();
}