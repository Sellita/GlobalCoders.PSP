using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;

public interface IMerchantService
{
    Task<bool> UpdateAsync(MerchantEntity updateModel);
    Task<bool> CreateAsync(MerchantEntity createModel);
    Task<BasePagedResponse<OrganizationsListModel>> GetAllAsync(OrganizationsFilter filter);
    Task<OrganizationResponseModel?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
}