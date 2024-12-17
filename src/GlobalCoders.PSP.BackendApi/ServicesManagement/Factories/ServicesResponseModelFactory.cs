using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Factories;

public static class ServicesResponseModelFactory
{
    public static ServiceResponseModel Create(ServiceEntity serviceTypeEntity)
    {
        return new ServiceResponseModel
        {
            Id = serviceTypeEntity.Id,
            DisplayName = serviceTypeEntity.DisplayName,
            Description = serviceTypeEntity.Description,
            DurationMin = serviceTypeEntity.DurationMin,
            Price = serviceTypeEntity.Price,
            ServiceState = serviceTypeEntity.ServiceState,
            EmployeeId = serviceTypeEntity.EmployeeId,
            Employee = serviceTypeEntity.Employee?.Name ?? string.Empty,
            CreationDate = serviceTypeEntity.CreationDate,
            LastUpdateDate = serviceTypeEntity.LastUpdateDate
        };
    }
}