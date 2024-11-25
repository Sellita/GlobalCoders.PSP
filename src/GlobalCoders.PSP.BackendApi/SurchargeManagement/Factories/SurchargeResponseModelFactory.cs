using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;

public class SurchargeResponseModelFactory
{
    public static SurchargeResponseModel Create(SurchargeEntity surchargeEntity)
    {
        return new SurchargeResponseModel
        {
            Name = surchargeEntity.Name,
            Value = surchargeEntity.Value,
            Type = surchargeEntity.Type,
            CreationDateTime = surchargeEntity.CreationDateTime,
            Status = surchargeEntity.Status,
            Minute = surchargeEntity.Minute,
            Hour = surchargeEntity.Hour,
            DayOfMonth = surchargeEntity.DayOfMonth,
            Month = surchargeEntity.Month,
            DayOfWeek = surchargeEntity.DayOfWeek
        };
    }
}