using GlobalCoders.PSP.BackendApi.ReservationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Factories;

public static class ReservationResponseModelFactory
{
    public static ReservationResponseModel Create(ReservationEntity serviceTypeEntity)
    {
        return new ReservationResponseModel
        {
            Id = serviceTypeEntity.Id,
            DisplayName = serviceTypeEntity.DisplayName,
            Description = serviceTypeEntity.Description,
            DurationMin = serviceTypeEntity.DurationMin,
            Price = serviceTypeEntity.Price,
            EmployeeId = serviceTypeEntity.EmployeeId,
            Employee = serviceTypeEntity.Employee?.Name ?? string.Empty,
            CreationDate = serviceTypeEntity.CreateTime,
            AppointmentTime = serviceTypeEntity.ReservationTime,
            AppointmentEndTime = serviceTypeEntity.ReservationEndTime
        };
    }
}