using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Repositories;

public interface ITaxRepository
{
    Task<bool> UpdateAsync(TaxEntity updateModel);
    Task<bool> CreateAsync(TaxEntity createModel);
    Task<(List<TaxEntity> items, int totalItems)> GetAllAsync(TaxFilter filter);
    Task<TaxEntity?> GetAsync(Guid taxId);
    Task<bool> DeleteAsync(Guid taxId, Guid? merchantId);
}