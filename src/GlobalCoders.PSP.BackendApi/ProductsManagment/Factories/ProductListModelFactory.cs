using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Enum;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Factories;

public static class ProductListModelFactory
{
    public static ProductListModel Create(ProductEntity productEntity)
    {
        return new ProductListModel
        {
            Id = productEntity.Id,
            DisplayName = productEntity.DisplayName,
            
            Description = productEntity.Description,
            Stock = null,//todo retrieve from inventory
            TaxName = null, //todo retrieve from tax
            TaxValue = null, //todo retrieve from tax
            CategoryId = productEntity.ProductType.Id, 
            Category = productEntity.ProductType.DisplayName,
            Price = productEntity.Price,
            ProductState = productEntity.ProductState,
            MerchantId = productEntity.MerchantId,
            Merchant = productEntity.Merchant.DisplayName,
            CreationDate = productEntity.CreationDate,
            LastUpdateDate = productEntity.LastUpdateDate,
        };
    }

    public static ProductListModel Create (ProductResponseModel organization)
    {
        return new ProductListModel
        {
            Id = organization.Id,
            DisplayName = organization.DisplayName,
            Description = organization.Description,
            Stock = null,//todo retrieve from inventory
            TaxName = null, //todo retrieve from tax
            TaxValue = null, //todo retrieve from tax
            CategoryId = organization.CategoryId,
            Category = organization.Category,
            Price = organization.Price,
            ProductState = organization.ProductState,
            MerchantId = organization.MerchantId,
            Merchant = organization.Merchant,
            CreationDate = organization.CreationDate,
            LastUpdateDate = organization.LastUpdateDate,
        };
    }
}