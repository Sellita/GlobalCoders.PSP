using GlobalCoders.PSP.BackendApi.OrganizationManagment.Repositories;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Extensions;

public static class OrganizationServicesExtension
{
    public static void RegisterOrganizationServices(this IServiceCollection services)
    {
        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IMerchantService, MerchantService>();
    }
    
    public static void RegisterOrganizatio(this WebApplication app)
    {

        //todo implement me
      
    }
}