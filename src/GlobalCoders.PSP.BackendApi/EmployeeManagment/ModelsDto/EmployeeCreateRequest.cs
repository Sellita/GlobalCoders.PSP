using GlobalCoders.PSP.BackendApi.Base.Enums;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;

public class EmployeeCreateRequest
{
    public required string Email { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
    public bool IsActive { get; set; }
    public Guid OrganizationId { get; set; }

    public int Minute { get; set; }
    public int Hour { get; set; }
    public int DayOfMonth { get; set; }
    public Mounths Month { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}