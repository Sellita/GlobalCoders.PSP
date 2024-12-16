using GlobalCoders.PSP.BackendApi.ProductsManagement.Controllers;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Repositories;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Services;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Extensions;

public static class ProductManagementExtension
{
    public static void RegisterProductsManagmentServices(this IServiceCollection services)
    {
        services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
        services.AddScoped<IProductTypeService, ProductTypeService>();  
        
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        
        services.AddScoped<ProductController>();
    }
    
    public static void RegisterProductsManagment(this WebApplication app)
    {

        //todo implement me
      
    }
}