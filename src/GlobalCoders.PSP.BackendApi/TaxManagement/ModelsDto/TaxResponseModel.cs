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
    public string Minute { get; set; } = String.Empty;
    public string Hour { get; set; } = String.Empty;
    public string DayOfMonth { get; set; } = String.Empty;
    public string Month { get; set; } = String.Empty;
    public string DayOfWeek { get; set; } = String.Empty;
}