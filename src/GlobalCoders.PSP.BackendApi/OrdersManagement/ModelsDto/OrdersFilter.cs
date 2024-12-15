using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrdersFilter : BaseFilter
{
    public string Client { get; set; } = string.Empty;
    public Guid? MerchantId { get; set; }
    public OrderStatus? OrderStatus { get; set; }
}