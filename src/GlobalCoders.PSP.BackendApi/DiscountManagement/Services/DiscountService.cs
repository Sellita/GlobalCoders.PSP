using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Repositories;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;
    
    public DiscountService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<bool> UpdateAsync(DiscountEntity updateModel)
    {
        return await _discountRepository.UpdateAsync(updateModel);
    }

    public async Task<bool> CreateAsync(DiscountEntity createModel)
    {
        return await _discountRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<DiscountListModel>> GetAllAsync(DiscountFilter filter)
    {
        var entities = await _discountRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(DiscountListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<DiscountResponseModel?> GetAsync(Guid discountId, Guid? merchantId)
    {
        var entity = await _discountRepository.GetAsync(discountId);
        
        if(entity == null)
        {
            return null;
        }
        
        if(merchantId != null && entity.MerchantId != merchantId)
        {
            return null;
        }
        
        return DiscountResponseModelFactory.Create(entity);
    }

    public Task<bool> DeleteAsync(Guid discountId, Guid? merchantId)
    {
        return _discountRepository.DeleteAsync(discountId, merchantId);
    }
}