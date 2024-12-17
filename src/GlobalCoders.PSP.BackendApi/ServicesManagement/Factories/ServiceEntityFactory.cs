using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Factories;

public static class ServiceEntityFactory
{
    public static ServiceEntity Create(ServiceCreateModel organizationCreateModel)
    {
        return new ServiceEntity
        {
            DisplayName = organizationCreateModel.DisplayName,
            Description = organizationCreateModel.Description,
            Price = organizationCreateModel.Price,
            ServiceState = organizationCreateModel.ServiceState,
            DurationMin = organizationCreateModel.DurationMin,
            EmployeeId = organizationCreateModel.EmployeeId,
            CreationDate = DateTime.UtcNow,
            LastUpdateDate = DateTime.UtcNow,
            Image = organizationCreateModel.Image
        };
    }  
    
    public static ServiceEntity CreateUpdate(ServiceUpdateModel updateModel)
    {
        var merchantEntity = Create(updateModel);

        merchantEntity.Id = updateModel.Id;
        merchantEntity.LastUpdateDate = DateTime.UtcNow;

        return merchantEntity;
    }
}