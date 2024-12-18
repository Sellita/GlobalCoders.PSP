using GlobalCoders.PSP.BackendApi.PaymentsService.Models;

namespace GlobalCoders.PSP.BackendApi.PaymentsService.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
    }
    public Task<bool> PayAsync(PaymentData payment)
    {
        _logger.LogInformation("Received payment: {@Payment}", payment);
        
        
        throw new NotImplementedException();
    }
}