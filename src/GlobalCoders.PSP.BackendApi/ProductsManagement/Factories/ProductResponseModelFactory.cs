using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Factories;

public static class ProductResponseModelFactory
{
    public static ProductResponseModel Create(ProductEntity productTypeEntity)
    {
        return new ProductResponseModel
        {
            Id = productTypeEntity.Id,
            DisplayName = productTypeEntity.DisplayName,
            Description = productTypeEntity.Description,
            Stock = null, //todo add stock from inventory
            TaxName = null, // todo add tax name from tax
            TaxValue = null,// todo add tax from tax
            CategoryId = productTypeEntity.ProductTypeId,
            Category = productTypeEntity.ProductType?.DisplayName ?? string.Empty,
            Price = productTypeEntity.Price,
            ProductState = productTypeEntity.ProductState,
            MerchantId = productTypeEntity.MerchantId,
            Merchant = productTypeEntity.Merchant?.DisplayName ?? string.Empty,
            CreationDate = productTypeEntity.CreationDate,
            LastUpdateDate = productTypeEntity.LastUpdateDate,


        };
    }
}