using GlobalCoders.PSP.BackendApi.Inventory.Repositories;
using GlobalCoders.PSP.BackendApi.Inventory.Services;


namespace GlobalCoders.PSP.BackendApi.Inventory.Extensions;

public static class InventoryServicesExtension
{
    public static void RegisterOrganizationServices(this IServiceCollection services)
    {
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IInventoryService, InventoryService>();
    }
    
    public static void RegisterOrganizatio(this WebApplication app)
    {

        //todo implement me
      
    }
}