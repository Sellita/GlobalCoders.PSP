using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Factories;

public class TaxResponseModelFactory
{
    public static TaxResponseModel Create(TaxEntity taxEntity)
    {
        return new TaxResponseModel
        {
            Name = taxEntity.Name,
            Value = taxEntity.Value,
            Type = taxEntity.Type,
            CreationDateTime = taxEntity.CreationDateTime,
            Status = taxEntity.Status,
            Minute = taxEntity.Minute,
            Hour = taxEntity.Hour,
            DayOfMonth = taxEntity.DayOfMonth,
            Month = taxEntity.Month,
            DayOfWeek = taxEntity.DayOfWeek
        };
    }
}