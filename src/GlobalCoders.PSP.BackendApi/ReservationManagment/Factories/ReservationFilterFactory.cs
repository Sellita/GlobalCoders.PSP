using GlobalCoders.PSP.BackendApi.ReservationManagment.Enums;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Factories;

public static class ReservationFilterFactory
{
    public static ReservationFilter CreateForAllActiveItems(Guid employeeId, DateTime startTime, DateTime endTime)
    {
        return new ReservationFilter
        {
            EmployeeId = employeeId,
            StartDate = startTime,
            EndDate = endTime,
            ReservationStatus = ReservationStatus.Active,
            Page = 1,
            ItemsPerPage = Int32.MaxValue
        };
    }
}