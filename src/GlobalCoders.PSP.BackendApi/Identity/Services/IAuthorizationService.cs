using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.Identity.Services;

public interface IAuthorizationService
{
    Task<Dictionary<string, bool>> CheckUserAccessAsync(
        EmployeeEntity appUser,
        IEnumerable<string> scopes,
        CancellationToken cancellationToken);
}
