using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Factories;

public static class SurchargeEntityFactory
{
    public static SurchargeEntity Create(SurchargeCreateModel surchargeEntity)
    {
        return new SurchargeEntity
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
    
    public static SurchargeEntity CreateUpdate(SurchargeUpdateModel updateModel)
    {
        var surchargeEntity = Create(updateModel);

        surchargeEntity.Id = updateModel.Id;
        
        return surchargeEntity;
    }
}