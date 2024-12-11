using GlobalCoders.PSP.BackendApi.DiscountManagment.Repositories;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Services;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Extensions;

public static class DiscountManagementExtension
{
    public static void RegisterDiscountManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IDiscountService, DiscountService>();
    }


}