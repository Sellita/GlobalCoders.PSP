using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Factories;

public class TaxListModelFactory
{
    public static TaxListModel Create(TaxEntity taxEntity)
    {
        return new TaxListModel()
        {
            Id = taxEntity.Id,
            DisplayName = taxEntity.Name
        };
    }
}