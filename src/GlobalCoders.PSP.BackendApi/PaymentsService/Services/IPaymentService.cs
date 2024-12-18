using GlobalCoders.PSP.BackendApi.PaymentsService.Models;

namespace GlobalCoders.PSP.BackendApi.PaymentsService.Services;

public interface IPaymentService
{
    public Task<PaymentInfo?> PayAsync(PaymentData payment);
}