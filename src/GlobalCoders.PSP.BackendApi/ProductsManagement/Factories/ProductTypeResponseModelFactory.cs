using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Factories;

public static class ProductTypeResponseModelFactory
{
    public static ProductTypeResponseModel Create(ProductTypeEntity productTypeEntity)
    {
        return new ProductTypeResponseModel
        {
            Id = productTypeEntity.Id,
            DisplayName = productTypeEntity.DisplayName
        };
    }
}