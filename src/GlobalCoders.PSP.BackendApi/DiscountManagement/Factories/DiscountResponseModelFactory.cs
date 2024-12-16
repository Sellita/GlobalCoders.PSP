using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;

public static class DiscountResponseModelFactory
{
    public static DiscountResponseModel Create(DiscountEntity discountEntity)
    {
        return new DiscountResponseModel
        {
            Id = discountEntity.Id,
            Name = discountEntity.Name,
            Value = discountEntity.Value,
            Type = discountEntity.Type,
            CreationDateTime = discountEntity.CreationDateTime,
            Status = discountEntity.Status,
            StartDate = discountEntity.StartDate,
            EndDate = discountEntity.EndDate,
            ProductTypeId = discountEntity.ProductTypeId,
            ProductTypeName = discountEntity.ProductType?.DisplayName,
            ProductId = discountEntity.ProductId,
            ProductName = discountEntity.Product?.DisplayName,
            MerchantId = discountEntity.MerchantId,
            MerchantName = discountEntity.Merchant?.DisplayName,

        };
    }
}