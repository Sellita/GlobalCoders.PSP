using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Factories;

public static class ProductTypeEntityFactory
{
    public static ProductTypeEntity Create(ProductTypeCreateModel organizationCreateModel)
    {
        return new ProductTypeEntity
        {
            DisplayName = organizationCreateModel.DisplayName
        };
    }  
    
    public static ProductTypeEntity CreateUpdate(ProductTypeUpdateModel updateModel)
    {
        var merchantEntity = Create(updateModel);

        merchantEntity.Id = updateModel.Id;
        
        return merchantEntity;
    }
}