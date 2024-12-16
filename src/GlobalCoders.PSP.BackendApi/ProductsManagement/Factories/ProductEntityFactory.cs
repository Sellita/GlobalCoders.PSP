using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Enum;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Factories;

public static class ProductEntityFactory
{
    public static ProductEntity Create(ProductCreateModel organizationCreateModel)
    {
        return new ProductEntity
        {
            DisplayName = organizationCreateModel.DisplayName,
            Description = organizationCreateModel.Description,
            Price = organizationCreateModel.Price,
            ProductState = ProductState.Active,
            ProductTypeId = organizationCreateModel.ProductTypeId,
            MerchantId = organizationCreateModel.MerchantId,
            CreationDate = DateTime.UtcNow,
            LastUpdateDate = DateTime.UtcNow,
            Image = organizationCreateModel.Image
        };
    }  
    
    public static ProductEntity CreateUpdate(ProductUpdateModel updateModel)
    {
        var merchantEntity = Create(updateModel);

        merchantEntity.Id = updateModel.Id;
        merchantEntity.LastUpdateDate = DateTime.UtcNow;

        return merchantEntity;
    }
}