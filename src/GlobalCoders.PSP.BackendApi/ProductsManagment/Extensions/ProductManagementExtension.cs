using GlobalCoders.PSP.BackendApi.ProductsManagment.Repositories;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Services;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Extensions;

public static class ProductManagementExtension
{
    public static void RegisterProductsMAnagmentServices(this IServiceCollection services)
    {
        services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
        services.AddScoped<IProductTypeService, ProductTypeService>();
    }
    
    public static void RegisterProductsManagment(this WebApplication app)
    {

        //todo implement me
      
    }
}