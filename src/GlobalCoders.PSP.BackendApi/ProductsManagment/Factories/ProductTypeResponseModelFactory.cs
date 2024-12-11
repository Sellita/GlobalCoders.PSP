using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Factories;

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