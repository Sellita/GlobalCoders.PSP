using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

public class ReservationListModel
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public int DurationMin { get; set; }
    
    public decimal Price { get; set; }
    public Guid EmployeeId { get; set; }
    public string Employee { get; set; } = string.Empty;
    
    public Guid MerchantId { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    
    public DateTime CreationDate { get; set; }
    public DateTime AppointmentTime { get; set; }
    public DateTime AppointmentEndTime { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}