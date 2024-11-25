using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Factories;

public class SurchargeListModelFactory
{
    public static SurchargeListModel Create(SurchargeEntity surchargeEntity)
    {
        return new SurchargeListModel()
        {
            Id = surchargeEntity.Id,
            DisplayName = surchargeEntity.Name
        };
    }
}