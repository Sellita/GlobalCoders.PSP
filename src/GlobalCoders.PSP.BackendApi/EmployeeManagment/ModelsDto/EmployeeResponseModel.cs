using GlobalCoders.PSP.BackendApi.Base.Enums;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;

public class EmployeeResponseModel
{
    public Guid EmployeeId { get; set; }
    public Guid MerchantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
    public bool IsActive { get; set; }
    public Guid OrganizationId { get; set; }
    public bool Hourly { get; set; }
    public bool Daily { get; set; }
    public bool Weekly { get; set; }
    public bool Monthly { get; set; }
    public bool Annually { get; set; }
    public int Minute { get; set; }
    public int Hour { get; set; }
    public int DayOfMonth { get; set; }
    public Mounths Month { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}