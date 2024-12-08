using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Factories;

public static class EmployeeResponseListModelFactory
{
    public static EmployeeResponseListModel Create(EmployeeEntity employeeEntity)
    {
        return new EmployeeResponseListModel
        {
            EmployeeId = employeeEntity.Id,
            Name = employeeEntity.Name,
            Email = employeeEntity.Email ?? string.Empty,
            Phone = employeeEntity.PhoneNumber ?? string.Empty,
            Role = employeeEntity.UserPermissions.FirstOrDefault()?.AppRole?.Name ?? string.Empty,
            CreateTime = employeeEntity.CreationDateTime,
            IsActive = employeeEntity.IsActive,
            MerchantId = employeeEntity.MerchantId ?? Guid.Empty
        };
    }
}