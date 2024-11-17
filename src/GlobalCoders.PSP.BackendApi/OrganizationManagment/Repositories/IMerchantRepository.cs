using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Repositories;

public interface IMerchantRepository
{
    Task<bool> UpdateAsync(MerchantEntity updateModel);
    Task<bool> CreateAsync(MerchantEntity createModel);
    Task<(List<MerchantEntity> items, int totalItems)> GetAllAsync(OrganizationsFilter filter);
    Task<MerchantEntity?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
}