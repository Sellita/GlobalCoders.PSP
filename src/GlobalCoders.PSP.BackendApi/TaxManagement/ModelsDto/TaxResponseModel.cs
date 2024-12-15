using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

public class TaxResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public TaxType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime CreationDateTime { get; set; }
    public TaxStatus Status { get; set; }
    
    public Guid? ProductTypeId { get; set; }
    public string? ProductTypeName { get; set; }
    
    public Guid MerchantId { get; set; }
    public string? MerchantName { get; set; }
}