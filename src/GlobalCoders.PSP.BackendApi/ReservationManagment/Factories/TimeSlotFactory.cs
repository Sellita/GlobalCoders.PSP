using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Factories;

public static class TimeSlotFactory
{
    public static TimeSlot Create(DateTime startDate, DateTime endDate)
    {
        var minutesDiff = (int)(endDate - startDate).TotalMinutes;
      
        return new TimeSlot
        {
            Time = startDate,
            DurationMin = minutesDiff
        };
    }
}