using GlobalCoders.PSP.BackendApi.SurchargeManagement.Enums;
namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

public class SurchargeCreateModel
{
    public string Name { get; set; } = String.Empty;
    public SurchargeValue Value { get; set; }
    public string Type { get; set; } = String.Empty;// "Percentage" or "Value"
    public DateTime CreationDateTime { get; set; } // When it was created
    public SurchargeStatus Status { get; set; } = SurchargeStatus.Active; // "Active" or "Inactive"
    public string Minute { get; set; } = String.Empty;
    public string Hour { get; set; } = String.Empty;
    public string DayOfMonth { get; set; } = String.Empty;
    public string Month { get; set; } = String.Empty;
    public string DayOfWeek { get; set; } = String.Empty;
}