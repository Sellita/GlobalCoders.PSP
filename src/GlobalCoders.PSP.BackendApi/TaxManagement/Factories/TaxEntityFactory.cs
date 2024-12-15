using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Factories;

public static class TaxEntityFactory
{
    public static TaxEntity Create(TaxCreateModel taxEntity, Guid? organizationId = null)
    {
        return new TaxEntity
        {
            MerchantId = taxEntity.OrganizationId ??
                         organizationId ?? throw new ArgumentNullException(nameof(organizationId)),
            Name = taxEntity.Name,
            ProductTypeId = taxEntity.ProductTypeId,
            Type = taxEntity.Type,
            Value = taxEntity.Value,
            CreationDateTime = taxEntity.CreationDateTime,
            Status = taxEntity.Status
        };
    }  
    
    public static TaxEntity CreateUpdate(TaxUpdateModel updateModel)
    {
        var taxEntity = Create(updateModel);

        taxEntity.Id = updateModel.Id;
        
        return taxEntity;
    }
}