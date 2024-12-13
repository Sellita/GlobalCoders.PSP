using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.Factories;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.TaxManagement.Repositories;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Services;

public class TaxService : ITaxService
{
    private readonly ITaxRepository _taxRepository;
    
    public TaxService(ITaxRepository taxRepository)
    {
        _taxRepository = taxRepository;
    }

    public async Task<bool> UpdateAsync(TaxEntity updateModel)
    {
        return await _taxRepository.UpdateAsync(updateModel);
    }

    public async Task<bool> CreateAsync(TaxEntity createModel)
    {
        return await _taxRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<TaxListModel>> GetAllAsync(TaxFilter filter)
    {
        var entities = await _taxRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(TaxListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);    }

    public async Task<TaxResponseModel?> GetAsync(Guid taxId)
    {
        var entity = await _taxRepository.GetAsync(taxId);
        
        return TaxResponseModelFactory.Create(entity);    }

    public Task<bool> DeleteAsync(Guid taxId)
    {
        return _taxRepository.DeleteAsync(taxId);
    }
}