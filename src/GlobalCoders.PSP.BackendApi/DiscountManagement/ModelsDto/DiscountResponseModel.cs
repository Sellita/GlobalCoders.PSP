using GlobalCoders.PSP.BackendApi.DiscountManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

public class DiscountResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DiscountStatus Status { get; set; }
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public Guid? ProductTypeId { get; set; }
    public string? ProductTypeName { get; set; }  
    
    public Guid? ProductId { get; set; }
    public string? ProductName { get; set; }
    
    public Guid MerchantId { get; set; }
    public string? MerchantName { get; set; }
}