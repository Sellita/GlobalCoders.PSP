using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Services;

public interface ITaxService
{
    Task<bool> UpdateAsync(TaxEntity updateModel);
    Task<bool> CreateAsync(TaxEntity createModel);
    Task<BasePagedResponse<TaxListModel>> GetAllAsync(TaxFilter filter);
    Task<TaxResponseModel?> GetAsync(Guid taxId);
    Task<bool> DeleteAsync(Guid taxId);
}