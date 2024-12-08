using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Services;

public interface ISurchargeService
{
    Task<bool> UpdateAsync(SurchargeEntity updateModel);
    Task<bool> CreateAsync(SurchargeEntity createModel);
    Task<BasePagedResponse<SurchargeListModel>> GetAllAsync(SurchargeFilter filter);
    Task<SurchargeResponseModel?> GetAsync(Guid surchargeId);
    Task<bool> DeleteAsync(Guid surchargeId);
}