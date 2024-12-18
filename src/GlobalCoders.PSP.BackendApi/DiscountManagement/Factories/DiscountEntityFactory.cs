using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;

public static class DiscountEntityFactory
{
    public static DiscountEntity Create(DiscountCreateModel discountCreateModel, Guid? organizationId = null)
    {
        var result = new DiscountEntity
        {
            MerchantId = discountCreateModel.OrganizationId ??
                         organizationId ?? throw new ArgumentNullException(nameof(organizationId)),
            Name = discountCreateModel.Name,
            ProductTypeId = discountCreateModel.ProductTypeId,
            ProductId = discountCreateModel.ProductId,
            Type = discountCreateModel.Type,
            Value = discountCreateModel.Value,
            CreationDateTime = discountCreateModel.CreationDateTime,
            Status = discountCreateModel.Status,
            StartDate = discountCreateModel.StartDate,
            EndDate = discountCreateModel.EndDate,
        };
        
        if(result.StartDate.HasValue)
        {
            result.StartDate = DateTime.SpecifyKind(result.StartDate.Value, DateTimeKind.Utc);
        }
        
        if(result.EndDate.HasValue)
        {
            result.EndDate = DateTime.SpecifyKind(result.EndDate.Value, DateTimeKind.Utc);
        }

        return result;
    }  
    
    public static DiscountEntity CreateUpdate(DiscountUpdateModel updateModel)
    {
        var discountEntity = Create(updateModel);

        discountEntity.Id = updateModel.Id;
        
        return discountEntity;
    }
}