using GlobalCoders.PSP.BackendApi.OrdersManagement.Repositories;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Services;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Extensions;

public static class OrdersServicesExtension
{
    public static void RegisterOrdersServices(this IServiceCollection services)
    {
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IOrdersService, OrdersService>();
    }
    
    public static void RegisterOrganizatio(this WebApplication app)
    {

        //todo implement me
      
    }
}