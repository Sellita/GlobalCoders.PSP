using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Repositories;

public interface ISurchargeRepository
{
    Task<bool> UpdateAsync(SurchargeEntity updateModel);
    Task<bool> CreateAsync(SurchargeEntity createModel);
    Task<(List<SurchargeEntity> items, int totalItems)> GetAllAsync(SurchargeFilter filter);
    Task<SurchargeEntity?> GetAsync(Guid surchargeId);
    Task<bool> DeleteAsync(Guid surchargeId);
}