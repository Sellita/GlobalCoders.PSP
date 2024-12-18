using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Enums;

namespace GlobalCoders.PSP.BackendApi.Identity.Services;

public interface IAuthorizationService
{
    Task<bool> HasPermissionsAsync(
        ClaimsPrincipal user,
        IEnumerable<Permissions> permissions,
        CancellationToken cancellationToken);
    
    Task<Dictionary<string, bool>> CheckUserAccessAsync(
        EmployeeEntity appUser,
        IEnumerable<string> scopes,
        CancellationToken cancellationToken);

    Task<EmployeeEntity?> GetUserAsync(ClaimsPrincipal user);
    Task<EmployeeEntity?> GetUserByIdAsync(Guid employeeId);
}
