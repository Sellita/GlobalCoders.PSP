namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderProductsModel
{
    public Guid Id { get; set; }
    
    public string ProductName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
}