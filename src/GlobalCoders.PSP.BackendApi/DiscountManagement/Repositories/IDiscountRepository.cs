using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;

public interface IDiscountRepository
{
    Task<(List<DiscountEntity> items, int totalItems)> GetFilteredAsync(DiscountFilter filter);
    Task CreateAsync(DiscountEntity discount);
    Task UpdateAsync(DiscountEntity discount);
    Task DeleteAsync(Guid id);
    Task<DiscountEntity?> GetAsync(Guid id);
    Task<List<DiscountEntity>> GetAllAsync();
}
