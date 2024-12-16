using GlobalCoders.PSP.BackendApi.DiscountManagement.Repositories;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Services;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Extensions;

public static class DiscountManagementExtension
{
    public static void RegisterDiscountManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
    }
    
    public static void RegisterDiscountManagement(this WebApplication app)
    {

        //todo implement me
      
    }
}