using GlobalCoders.PSP.BackendApi.DiscountManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

public class DiscountListModel
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public DiscountType Type { get; set; } 
    public decimal Value { get; set; }
    public Guid? ProductTypeId { get; set; }
    public Guid? ProductId { get; set; }
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}