using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Factories;

public static class OrganizationResponseModelFactory
{
    public static OrganizationResponseModel Create(MerchantEntity merchantEntity)
    {
        return new OrganizationResponseModel
        {
            Id = merchantEntity.Id,
            DisplayName = merchantEntity.DisplayName,
            LegalName = merchantEntity.LegalName,
            Address = merchantEntity.Address,
            Email = merchantEntity.Email,
            MainPhoneNumber = merchantEntity.MainPhoneNr,
            SecondaryPhoneNumber = merchantEntity.SecondaryPhoneNr,
            WorkingSchedule = merchantEntity.WorkingSchedule.Select(x => new OrganizationScheduleRequest()
            {
                DayOfWeek = x.DayOfWeek,
                StartTime = x.StartTime,
                EndTime = x.EndTime
            }).ToList()

        };
    }
}