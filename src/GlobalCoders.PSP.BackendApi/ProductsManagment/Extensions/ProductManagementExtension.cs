using GlobalCoders.PSP.BackendApi.ProductsManagment.Controllers;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Repositories;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Services;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Extensions;

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