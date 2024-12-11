using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Services;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<bool> CreateAsync(DiscountCreateModel model)
    {
        var discount = DiscountEntityFactory.Create(model);
        return await _discountRepository.CreateAsync(discount);
    }

    public async Task<bool> UpdateAsync(DiscountUpdateModel model)
    {
        var discount = DiscountEntityFactory.Update(model);
        return await _discountRepository.UpdateAsync(discount);
    }

    public async Task<BasePagedResponse<DiscountListModel>> GetAllAsync(DiscountFilter filter)
    {
        var (items, total) = await _discountRepository.GetAllAsync(filter);
        return BasePagedResponseFactory.Create(
            items.Select(DiscountListModelFactory.Create).ToList(),
            filter,
            total
        );
    }
    
    public async Task<DiscountResponseModel?> GetAsync(Guid id)
    {
        var entity = await _discountRepository.GetAsync(id);
        if (entity == null)
        {
            return null; // Or throw an exception, based on your application's needs
        }

        return DiscountResponseModelFactory.Create(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _discountRepository.DeleteAsync(id);
    }
}