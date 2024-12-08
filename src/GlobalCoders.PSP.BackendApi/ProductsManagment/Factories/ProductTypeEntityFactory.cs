using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Factories;

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