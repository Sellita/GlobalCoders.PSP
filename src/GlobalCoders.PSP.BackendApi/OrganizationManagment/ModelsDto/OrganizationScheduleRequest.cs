namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

public class OrganizationScheduleRequest
{
    public DayOfWeek DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}