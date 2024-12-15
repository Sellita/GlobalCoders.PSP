namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;

public class EmployeeScheduleRequest
{
    public DayOfWeek DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}