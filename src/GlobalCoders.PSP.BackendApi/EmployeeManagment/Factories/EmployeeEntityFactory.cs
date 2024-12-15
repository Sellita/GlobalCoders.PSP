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
            PhoneNumber = request.PhoneNumber,
            CreationDateTime = DateTime.UtcNow,
            WorkingSchedule = request.WorkingSchedule.Select(x=> new EmployeeScheduleEntity
            {
                DayOfWeek = x.DayOfWeek,
                StartTime = x.StartTime,
                EndTime = x.EndTime
            }).ToList(),
            MerchantId = request.OrganizationId
        };
    }
}