using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Factories;

public static class TaxResponseModelFactory
{
    public static TaxResponseModel Create(TaxEntity taxEntity)
    {
        return new TaxResponseModel
        {
            Id = taxEntity.Id,
            Name = taxEntity.Name,
            Value = taxEntity.Value,
            Type = taxEntity.Type,
            CreationDateTime = taxEntity.CreationDateTime,
            Status = taxEntity.Status,
            ProductTypeId = taxEntity.ProductTypeId,
            ProductTypeName = taxEntity.ProductType?.DisplayName,
            MerchantId = taxEntity.MerchantId,
            MerchantName = taxEntity.Merchant?.DisplayName,

        };
    }
}