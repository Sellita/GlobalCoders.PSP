using GlobalCoders.PSP.BackendApi.InventoryManagement.Repositories;
using GlobalCoders.PSP.BackendApi.InventoryManagement.Services;

namespace GlobalCoders.PSP.BackendApi.InventoryManagement.Extensions;

public static class InventoryServicesExtension
{
    public static void RegisterInventoryServices(this IServiceCollection services)
    {
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IInventoryService, InventoryService>();
    }
    
    public static void RegisterInventory(this WebApplication app)
    {

        //todo implement me
      
    }
}