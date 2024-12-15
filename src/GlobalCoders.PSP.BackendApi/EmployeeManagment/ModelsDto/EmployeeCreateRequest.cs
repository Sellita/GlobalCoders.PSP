using GlobalCoders.PSP.BackendApi.Base.Enums;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;

public class EmployeeCreateRequest
{
    public required string Email { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid OrganizationId { get; set; }

    public List<EmployeeScheduleRequest> WorkingSchedule { get; set; } = new ();
}