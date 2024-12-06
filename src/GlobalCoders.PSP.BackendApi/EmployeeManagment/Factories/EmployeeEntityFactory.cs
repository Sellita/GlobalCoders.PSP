using GlobalCoders.PSP.BackendApi.Base.Enums;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Factories;

public static class EmployeeEntityFactory
{
    public static EmployeeEntity Create(EmployeeCreateRequest request)
    {
        return new EmployeeEntity
        {
            UserName = request.Email,
            NormalizedUserName = request.Email.ToUpper(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpper(),
            EmailConfirmed = true,
            IsActive = request.IsActive,
            // for employee
            Name = request.Name,
            CreationDateTime = request.CreateTime,
            Minute = request.Minute,
            Hour = request.Hour,
            DayMounth = request.DayOfMonth,
            Mounth = request.Month,
            DayWeek = request.DayOfWeek,
            MerchantId = request.OrganizationId
        };
    }
}