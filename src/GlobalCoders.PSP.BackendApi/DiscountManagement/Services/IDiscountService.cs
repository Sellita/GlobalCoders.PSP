using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Services;

public interface IDiscountService
{
    Task<bool> UpdateAsync(DiscountEntity updateModel);
    Task<bool> CreateAsync(DiscountEntity createModel);
    Task<BasePagedResponse<DiscountListModel>> GetAllAsync(DiscountFilter filter);
    Task<DiscountResponseModel?> GetAsync(Guid discountId, Guid? merchantId);
    Task<bool> DeleteAsync(Guid discountId, Guid? merchantId);
}