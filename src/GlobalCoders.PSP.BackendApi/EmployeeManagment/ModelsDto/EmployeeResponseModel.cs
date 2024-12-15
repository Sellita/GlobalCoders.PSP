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
    
    public List<EmployeeScheduleRequest> WorkingSchedule { get; set; } = new ();

}