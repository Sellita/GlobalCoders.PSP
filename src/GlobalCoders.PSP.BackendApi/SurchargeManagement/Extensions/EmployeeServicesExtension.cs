using GlobalCoders.PSP.BackendApi.SurchargeManagement.Repositories;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Services;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Extensions;

public static class SurChargeManagementExtension
{
    public static void RegisterSurChargeManagementServices(this IServiceCollection services)
    {
        //todo implement me
        services.AddScoped<ISurchargeService, SurchargeService>();
        services.AddScoped<ISurchargeRepository, SurchargeRepository>();
    }
    
    public static void RegisterSurChargeManagement(this WebApplication app)
    {

        //todo implement me
      
    }
}