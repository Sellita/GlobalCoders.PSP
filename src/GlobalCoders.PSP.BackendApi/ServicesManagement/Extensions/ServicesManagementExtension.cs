using GlobalCoders.PSP.BackendApi.ServicesManagement.Controllers;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Repositories;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Services;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Extensions;

public static class ServicesManagementExtension
{
    public static void RegisterServicesManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IServicesRepository, ServicesRepository>();
        services.AddScoped<IServicesService, ServicesService>();
        
        services.AddScoped<ServiceController>();
    }
    
    public static void RegisterServicessManagement(this WebApplication app)
    {

        //todo implement me
      
    }
}