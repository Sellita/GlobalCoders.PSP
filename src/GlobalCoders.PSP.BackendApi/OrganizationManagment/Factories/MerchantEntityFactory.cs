using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Factories;

public static class MerchantEntityFactory
{
    public static MerchantEntity Create(OrganizationCreateModel organizationCreateModel)
    {
        return new MerchantEntity
        {
            DisplayName = organizationCreateModel.DisplayName,
            LegalName = organizationCreateModel.LegalName,
            Address = organizationCreateModel.Address,
            Email = organizationCreateModel.Email,
            MainPhoneNr = organizationCreateModel.MainPhoneNumber,
            SecondaryPhoneNr = organizationCreateModel.SecondaryPhoneNumber,
            WorkingSchedule = organizationCreateModel.WorkingSchedule.Select(x => new OrganizationScheduleEntity
            {
                DayOfWeek = x.DayOfWeek,
                StartTime = x.StartTime,
                EndTime = x.EndTime
            }).ToList()
        };
    }  
    
    public static MerchantEntity CreateUpdate(OrganizationUpdateModel updateModel)
    {
        var merchantEntity = Create(updateModel);

        merchantEntity.Id = updateModel.Id;
        
        return merchantEntity;
    }
}