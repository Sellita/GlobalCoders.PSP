namespace GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

public class TimeSlotRequest
{
    public DateTime DateTime { get; set; }
    public Guid EmployeeId { get; set; }
    public int? MinimumDurationMin { get; set; }
}