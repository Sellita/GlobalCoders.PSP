using System.Net;
using GlobalCoders.PSP.BackendApi.PaymentsService.Configuration;
using GlobalCoders.PSP.BackendApi.PaymentsService.Services;
using Stripe;

namespace GlobalCoders.PSP.BackendApi.PaymentsService.Extensions;

public static class PaymentManagementExtension
{
    public static void RegisterPaymentManagementServices(this IServiceCollection services, IConfiguration configuration)
    {
      //  services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, PaymentService>();

        var identityConfiguration =
            configuration.GetSection(PaymentConfiguration.SectionName).Get<PaymentConfiguration>();

        if (identityConfiguration == null)
        {
            throw new InvalidOperationException("Payment configuration is missing");
        }

        StripeConfiguration.ApiKey = identityConfiguration.ApiKey;
    }

    public static void RegisterPaymentManagement(this WebApplication app)
    {
        //todo implement me
    }
}