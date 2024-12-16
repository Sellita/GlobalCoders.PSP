using GlobalCoders.PSP.BackendApi.DiscountManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

public class DiscountCreateModel
{
    public string Name { get; set; } = String.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime CreationDateTime { get; set; } 
    public DiscountStatus Status { get; set; } = DiscountStatus.Active; 
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? OrganizationId { get; set; }
    public Guid? ProductTypeId { get; set; }
    public Guid? ProductId { get; set; }
}