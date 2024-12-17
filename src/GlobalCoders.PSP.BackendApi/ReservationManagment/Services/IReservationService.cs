namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Services;

public interface IReservationService
{
    Task<object?> GetAsync(Guid reservationId, Guid? merchantId);
}