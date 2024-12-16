using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Factories;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Repositories;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;

public class MerchantService : IMerchantService
{
    private readonly IMerchantRepository _merchantRepository;

    public MerchantService(IMerchantRepository merchantRepository)
    {
        _merchantRepository = merchantRepository;
    }
    public async Task<bool> UpdateAsync(MerchantEntity updateModel)
    {
        return await _merchantRepository.UpdateAsync(updateModel);
    }

    public async Task<bool> CreateAsync(MerchantEntity createModel)
    {
        return await _merchantRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<OrganizationsListModel>> GetAllAsync(OrganizationsFilter filter)
    {
        var entities = await _merchantRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(OrganizationsListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<OrganizationResponseModel?> GetAsync(Guid organizationId)
    {
        var entity = await _merchantRepository.GetAsync(organizationId);

        if (entity == null)
        {
            return null;
        }
        
        return OrganizationResponseModelFactory.Create(entity);
    }

    public Task<bool> DeleteAsync(Guid organizationId)
    {
        return _merchantRepository.DeleteAsync(organizationId);
    }
}