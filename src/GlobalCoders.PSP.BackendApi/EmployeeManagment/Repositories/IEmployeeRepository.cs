using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Repositories;

public interface IEmployeeRepository
{
    Task<bool> UpdateAsync(EmployeeEntity appUser, string updateRequestRole, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid employeeId);
    Task<EmployeeEntity?> GetUserAsync(Guid userId);
    Task<bool> UpdateScheduleAsync(EmployeeEntity user, CancellationToken cancellationToken);
}