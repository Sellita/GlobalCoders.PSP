using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderChangeStatusRequestModel
{
    public Guid OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
}