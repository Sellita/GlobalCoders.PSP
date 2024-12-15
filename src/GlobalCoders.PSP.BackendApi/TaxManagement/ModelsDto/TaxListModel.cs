using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

public class TaxListModel
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public TaxType Type { get; set; } 
    public decimal Value { get; set; }
    public Guid? ProductTypeId { get; set; }
}