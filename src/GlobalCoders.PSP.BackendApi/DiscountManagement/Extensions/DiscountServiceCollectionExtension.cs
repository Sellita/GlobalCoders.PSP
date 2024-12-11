using GlobalCoders.PSP.BackendApi.DiscountManagment.Repositories;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Extensions;

public static class DiscountServiceCollectionExtensions
{
    public static IServiceCollection RegisterDiscountServices(this IServiceCollection services)
    {
        
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IDiscountService, DiscountService>();

        return services; 
    }
}