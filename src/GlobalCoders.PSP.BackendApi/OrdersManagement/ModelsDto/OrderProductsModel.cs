using GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderProductsModel
{
    public Guid Id { get; set; }
    
    public string ProductName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public OrderTaxModel[] Tax { get; set; } = Array.Empty<OrderTaxModel>();
    public decimal Discount { get; set; }
}