using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

public class ReservationCreateModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid EmployeeId { get; set; } 
    
    public Guid Service { get; set; } 
    
    public DateTime AppointmentTime { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}