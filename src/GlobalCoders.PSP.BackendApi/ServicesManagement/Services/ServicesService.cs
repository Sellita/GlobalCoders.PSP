using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Factories;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Repositories;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Services;

public class ServicesService : IServicesService
{
    private readonly IServicesRepository _servicesRepository;

    public ServicesService(IServicesRepository servicesRepository)
    {
        _servicesRepository = servicesRepository;
    }
    public async Task<bool> UpdateAsync(ServiceEntity updateModel)
    {
        return await _servicesRepository.UpdateAsync(updateModel);
    }

    public async Task<bool> CreateAsync(ServiceEntity createModel)
    {
        return await _servicesRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<ServiceListModel>> GetAllAsync(ServiceFilter filter)
    {
        var entities = await _servicesRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(ServicesListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<ServiceResponseModel?> GetAsync(Guid serviceId, Guid? merchantId = null)
    {
        var entity = await _servicesRepository.GetAsync(serviceId, merchantId);
        
        if(entity == null)
        {
            return null;
        }
        
        return ServicesResponseModelFactory.Create(entity);
    }

    public Task<bool> DeleteAsync(Guid organizationId)
    {
        return _servicesRepository.DeleteAsync(organizationId);
    }
}