using GlobalCoders.PSP.BackendApi.ReservationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Factories;

public static class ReservationListModelFactory
{
    public static ReservationListModel Create(ReservationEntity serviceEntity)
    {
        return new ReservationListModel
        {
            Id = serviceEntity.Id,
            DisplayName = serviceEntity.DisplayName,
            Description = serviceEntity.Description,
            DurationMin = serviceEntity.DurationMin,
            Price = serviceEntity.Price,
            Status = serviceEntity.Status,
            CustomerName = serviceEntity.CustomerName,
            EmployeeId = serviceEntity.EmployeeId,
            Employee = serviceEntity.Employee?.Name ?? string.Empty,
            MerchantId = serviceEntity.Employee?.MerchantId ?? Guid.Empty,
            MerchantName = serviceEntity.Employee?.Merchant?.DisplayName ?? string.Empty,
            CreationDate = serviceEntity.CreateTime,
            AppointmentTime = serviceEntity.ReservationTime,
            AppointmentEndTime = serviceEntity.ReservationEndTime
        };
    }

    public static ReservationListModel Create (ReservationResponseModel service)
    {
        return new ReservationListModel
        {
            Id = service.Id,
            DisplayName = service.DisplayName,
            Description = service.Description,
            DurationMin = service.DurationMin,
            CustomerName = service.CustomerName,
            Status = service.Status,
            Price = service.Price,
            EmployeeId = service.EmployeeId,
            Employee = service.Employee,
            MerchantId = service.Merchant,
            MerchantName = service.MerchantName,
            CreationDate = service.CreationDate,
            AppointmentTime = service.AppointmentTime,
            AppointmentEndTime = service.AppointmentEndTime,

        };
    }
}