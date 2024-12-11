using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Factories;

public static class ProductTypeListModelFactory
{
    public static ProductTypeListModel Create(ProductTypeEntity merchantEntity)
    {
        return new ProductTypeListModel
        {
            Id = merchantEntity.Id,
            DisplayName = merchantEntity.DisplayName
        };
    }

    public static ProductTypeListModel Create (ProductTypeResponseModel organization)
    {
        return new ProductTypeListModel
        {
            Id = organization.Id,
            DisplayName = organization.DisplayName
        };
    }
}