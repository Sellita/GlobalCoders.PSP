using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderResponseModel
{
    public Guid Id { get; set; }
    public Guid MerchantId { get; set; } 
    public string MerchantName { get; set; } = string.Empty;
    public Guid EmployeeId { get; set; } 
    public string EmployeeName { get; set; } = string.Empty;

    public string ClientName { get; set; } = string.Empty; public decimal TotalTax { get; set; }
    public decimal Discount { get; set; }
    public decimal Price { get; set; }
    public decimal PriceWithTax { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal PaidSum { get; set; }
    public decimal Tips { get; set; }
    
    public OrderStatus Status { get; set; }

    public List<OrderProductsModel> Products { get; set; } = new ();
    public List<OrderPaymentsModel> Payments { get; set; } = new ();
    public List<OrderDiscountModel> Discounts { get; set; } = new ();
    
    public DateTime Date { get; set; }
}