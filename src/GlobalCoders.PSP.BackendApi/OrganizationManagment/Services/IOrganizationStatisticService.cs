using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;

public interface IOrganizationStatisticService
{
    Task<OrganizationDailyStatisticModel?> GetAsync(OrganizationStatisticRequest request);
}