using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrdersListModel
{
    public Guid Id { get; set; }
    public string Client { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    
    public string Merchant { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
}