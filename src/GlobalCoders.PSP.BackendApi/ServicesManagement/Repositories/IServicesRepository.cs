using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Repositories;

public interface IServicesRepository
{
    Task<bool> UpdateAsync(ServiceEntity updateModel);
    Task<bool> CreateAsync(ServiceEntity createModel);
    Task<(List<ServiceEntity> items, int totalItems)> GetAllAsync(ServiceFilter filter);
    Task<ServiceEntity?> GetAsync(Guid serviceId, Guid? merchantId);
    Task<bool> DeleteAsync(Guid organizationId);
}