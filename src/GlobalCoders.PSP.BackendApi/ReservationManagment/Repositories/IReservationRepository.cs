

using GlobalCoders.PSP.BackendApi.ReservationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Repositories;

public interface IReservationRepository
{
    Task<bool> UpdateAsync(ReservationEntity updateModel);
    Task<bool> CreateAsync(ReservationEntity createModel);
    Task<(List<ReservationEntity> items, int totalItems)> GetAllAsync(ReservationFilter filter);
    Task<ReservationEntity?> GetAsync(Guid serviceId, Guid? merchantId);
    Task<bool> DeleteAsync(Guid organizationId);
    Task<bool> IsAppointmentExistsAsync(Guid serviceUserId, DateTime appointmentTime, DateTime appointmentEndDate);
}