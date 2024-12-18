using GlobalCoders.PSP.BackendApi.OrdersManagement.Repositories;
using GlobalCoders.PSP.BackendApi.PaymentsService.Models;
using Stripe.Checkout;

namespace GlobalCoders.PSP.BackendApi.PaymentsService.Services;

public class PaymentService : IPaymentService
{
    private readonly ILogger<PaymentService> _logger;
    private readonly IOrdersRepository _ordersRepository;

    public PaymentService(ILogger<PaymentService> logger, IOrdersRepository ordersRepository)
    {
        _logger = logger;
        _ordersRepository = ordersRepository;
    }
    public async Task<PaymentInfo?> PayAsync(PaymentData payment)
    {
        _logger.LogInformation("Received payment: {@Payment}", payment);
        const string baseUrl = "http://localhost:9001/payments/";
        
        var stripeSessionService = new SessionService();
        var checkOutSession = await stripeSessionService.CreateAsync(
            CreateSessionOptions(payment, baseUrl));
        
        _logger.LogInformation("Created checkout session: {@CheckOutSession}", checkOutSession);

        if (checkOutSession == null)
        {
            return null;
        }
        
        await _ordersRepository.UpdatePaymentSessionIdAsync(payment.PaymentId, checkOutSession.Id);
        
        return new PaymentInfo()
        {
            PaymentUrl = checkOutSession.Url
        };
    }

    private static SessionCreateOptions CreateSessionOptions(PaymentData payment, string baseUrl)
    {
        return new SessionCreateOptions
        {
            Mode = "payment",
            ClientReferenceId = payment.PaymentId.ToString(),
            SuccessUrl = baseUrl +$"success?orderId={payment.OrderId}&sessionId={{CHECKOUT_SESSION_ID}}",
            CancelUrl = baseUrl + $"cancel?orderId={payment.OrderId}&sessionId={{CHECKOUT_SESSION_ID}}",
            LineItems = new()
            {
                new ()
                {
                    PriceData = new()
                    {
                        Currency = "EUR",
                        UnitAmount = (long)(payment.Amount * 100),//todo we can put here  products
                        ProductData = new()
                        {
                            Name = "Order payment"
                        }
                    },
                    Quantity = 1
                }
            }
        };
    }
}