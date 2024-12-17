using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Services;

public interface IServicesService
{
    Task<bool> UpdateAsync(ServiceEntity updateModel);
    Task<bool> CreateAsync(ServiceEntity createModel);
    Task<BasePagedResponse<ServiceListModel>> GetAllAsync(ServiceFilter filter);
    Task<ServiceResponseModel?> GetAsync(Guid serviceId, Guid? merchantId = null);
    Task<bool> DeleteAsync(Guid organizationId);
}