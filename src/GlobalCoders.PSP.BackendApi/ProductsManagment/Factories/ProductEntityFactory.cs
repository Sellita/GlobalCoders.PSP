using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Factories;

public static class ProductEntityFactory
{
    public static ProductEntity Create(ProductCreateModel organizationCreateModel)
    {
        return new ProductEntity
        {
            DisplayName = organizationCreateModel.DisplayName
        };
    }  
    
    public static ProductEntity CreateUpdate(ProductUpdateModel updateModel)
    {
        var merchantEntity = Create(updateModel);

        merchantEntity.Id = updateModel.Id;
        
        return merchantEntity;
    }
}