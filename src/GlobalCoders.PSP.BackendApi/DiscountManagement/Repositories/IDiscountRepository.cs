using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Repositories;

public interface IDiscountRepository
{
    Task<bool> UpdateAsync(DiscountEntity updateModel);
    Task<bool> CreateAsync(DiscountEntity createModel);
    Task<(List<DiscountEntity> items, int totalItems)> GetAllAsync(DiscountFilter filter);
    Task<DiscountEntity?> GetAsync(Guid discountId);
    Task<bool> DeleteAsync(Guid discountId, Guid? merchantId);
}