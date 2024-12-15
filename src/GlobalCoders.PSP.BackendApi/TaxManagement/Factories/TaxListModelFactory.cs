using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Factories;

public static class TaxListModelFactory
{
    public static TaxListModel Create(TaxEntity taxEntity)
    {
        return new TaxListModel
        {
            Id = taxEntity.Id,
            DisplayName = taxEntity.Name,
            Type = taxEntity.Type,
            Value = taxEntity.Value,
            ProductTypeId = taxEntity.ProductTypeId,

        };
    }
}