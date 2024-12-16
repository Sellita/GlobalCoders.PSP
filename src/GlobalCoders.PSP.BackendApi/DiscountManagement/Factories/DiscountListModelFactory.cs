using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;

public static class DiscountListModelFactory
{
    public static DiscountListModel Create(DiscountEntity discountEntity)
    {
        return new DiscountListModel
        {
            Id = discountEntity.Id,
            DisplayName = discountEntity.Name,
            Type = discountEntity.Type,
            Value = discountEntity.Value,
            ProductTypeId = discountEntity.ProductTypeId,
            ProductId = discountEntity.ProductId,
            StartDate = discountEntity.StartDate,
            EndDate = discountEntity.EndDate
        };
    }
}