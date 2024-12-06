using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Factories;

public static class OrganizationsListModelFactory
{
    public static OrganizationsListModel Create(MerchantEntity merchantEntity)
    {
        return new OrganizationsListModel
        {
            Id = merchantEntity.Id,
            DisplayName = merchantEntity.DisplayName
        };
    }

    public static OrganizationsListModel Create (OrganizationResponseModel organization)
    {
        return new OrganizationsListModel
        {
            Id = organization.Id,
            DisplayName = organization.DisplayName
        };
    }
}