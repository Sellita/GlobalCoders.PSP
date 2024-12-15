using GlobalCoders.PSP.BackendApi.TaxManagement.Repositories;
using GlobalCoders.PSP.BackendApi.TaxManagement.Services;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Extensions;

public static class TaxManagementExtension
{
    public static void RegisterTaxManagementServices(this IServiceCollection services)
    {
        services.AddScoped<ITaxService, TaxService>();
        services.AddScoped<ITaxRepository, TaxRepository>();
    }
    
    public static void RegisterTaxManagement(this WebApplication app)
    {

        //todo implement me
      
    }
}