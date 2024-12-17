using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Services;

public interface IReservationService
{
    Task<ReservationResponseModel?> GetAsync(Guid reservationId, Guid? merchantId);
    Task<BasePagedResponse<ReservationListModel>?> GetAllAsync(ReservationFilter filter);
    Task<(bool, string)> CreateAsync(ReservationCreateModel reservationCreateModel, EmployeeEntity serviceUser);
    Task<List<TimeSlot>?> GetTimeSlotsAsync(TimeSlotRequest request, EmployeeEntity user);
}