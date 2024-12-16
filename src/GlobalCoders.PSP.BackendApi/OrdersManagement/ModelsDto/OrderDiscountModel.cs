using GlobalCoders.PSP.BackendApi.DiscountManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderDiscountModel
{
    public Guid DiscountId { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public DiscountType Type { get; set; }
}