using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Enums;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Factories;

public static class ReservationEntityFactory
{
    public static ReservationEntity Create(
        ReservationCreateModel reservationCreateModel,
        ServiceResponseModel service, 
        EmployeeEntity serviceUser)
    {
        return new ReservationEntity
        {
            DisplayName = reservationCreateModel.DisplayName,
            Description = reservationCreateModel.Description,
            Price = service.Price,
            CreateTime = DateTime.UtcNow,
            Status = ReservationStatus.Active,
            CustomerName = reservationCreateModel.CustomerName,
            ReservationTime = reservationCreateModel.AppointmentTime,
            ReservationEndTime = reservationCreateModel.AppointmentTime.AddMinutes(service.DurationMin),
            DurationMin = service.DurationMin,
            ServiceId = service.Id,
            EmployeeId = serviceUser.Id,
            IsDeleted = false
        };
    }
}