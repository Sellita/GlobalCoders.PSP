using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Factories;

public static class ServicesListModelFactory
{
    public static ServiceListModel Create(ServiceEntity serviceEntity)
    {
        return new ServiceListModel
        {
            Id = serviceEntity.Id,
            DisplayName = serviceEntity.DisplayName,
            Description = serviceEntity.Description,
            DurationMin = serviceEntity.DurationMin,
            Price = serviceEntity.Price,
            ServiceState = serviceEntity.ServiceState,
            EmployeeId = serviceEntity.EmployeeId,
            Employee = serviceEntity.Employee?.Name ?? string.Empty,
            CreationDate = serviceEntity.CreationDate,
            LastUpdateDate = serviceEntity.LastUpdateDate
        };
    }

    public static ServiceListModel Create (ServiceResponseModel service)
    {
        return new ServiceListModel
        {
            Id = service.Id,
            DisplayName = service.DisplayName,
            Description = service.Description,
            DurationMin = service.DurationMin,
            Price = service.Price,
            ServiceState = service.ServiceState,
            EmployeeId = service.EmployeeId,
            Employee = service.Employee,
            CreationDate = service.CreationDate,
            LastUpdateDate = service.LastUpdateDate,
        };
    }
}