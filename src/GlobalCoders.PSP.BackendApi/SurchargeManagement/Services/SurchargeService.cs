using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Factories;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Repositories;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Services;

public class SurchargeService : ISurchargeService
{
    private readonly ISurchargeRepository _surchargeRepository;

    public SurchargeService(ISurchargeRepository surchargeRepository)
    {
        _surchargeRepository = surchargeRepository;
    }

    public async Task<bool> UpdateAsync(SurchargeEntity updateModel)
    {
        return await _surchargeRepository.UpdateAsync(updateModel);
    }

    public async Task<bool> CreateAsync(SurchargeEntity createModel)
    {
        return await _surchargeRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<SurchargeListModel>> GetAllAsync(SurchargeFilter filter)
    {
        var entities = await _surchargeRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(SurchargeListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<SurchargeResponseModel?> GetAsync(Guid surchargeId)
    {
        var entity = await _surchargeRepository.GetAsync(surchargeId);
        
        return SurchargeResponseModelFactory.Create(entity);
    }

    public Task<bool> DeleteAsync(Guid surchargeId)
    {
        return _surchargeRepository.DeleteAsync(surchargeId);
    }
}