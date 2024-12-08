using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

public class TaxCreateModel
{
    public string Name { get; set; } = String.Empty;
    public TaxType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime CreationDateTime { get; set; } // When it was created
    public TaxStatus Status { get; set; } = TaxStatus.Active; // "Active" or "Inactive"
    public string Minute { get; set; } = String.Empty;
    public string Hour { get; set; } = String.Empty;
    public string DayOfMonth { get; set; } = String.Empty;
    public string Month { get; set; } = String.Empty;
    public string DayOfWeek { get; set; } = String.Empty;
    public Guid? OrganizationId { get; set; }
}