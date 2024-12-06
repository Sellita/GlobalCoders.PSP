using GlobalCoders.PSP.BackendApi.SurchargeManagement.Enums;
namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

public class SurchargeResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public SurchargeType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime CreationDateTime { get; set; }
    public SurchargeStatus Status { get; set; }
    public string Minute { get; set; } = String.Empty;
    public string Hour { get; set; } = String.Empty;
    public string DayOfMonth { get; set; } = String.Empty;
    public string Month { get; set; } = String.Empty;
    public string DayOfWeek { get; set; } = String.Empty;
}