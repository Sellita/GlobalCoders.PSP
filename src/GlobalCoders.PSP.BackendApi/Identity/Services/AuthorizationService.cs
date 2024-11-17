using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.Identity.Services;

public sealed class AuthorizationService : IAuthorizationService
{
    private readonly ILogger<AuthorizationService> _logger;
    private readonly UserManager<EmployeeEntity> _userManager;
    private readonly RoleManager<PermisionTemplateEntity> _roleManager;

    public AuthorizationService(
        ILogger<AuthorizationService> logger,
        UserManager<EmployeeEntity> userManager,
        RoleManager<PermisionTemplateEntity> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Dictionary<string, bool>> CheckUserAccessAsync(
        EmployeeEntity appUser,
        IEnumerable<string> scopes,
        CancellationToken cancellationToken)
    {
        try
        {
            var userRoles = await _userManager.GetRolesAsync(appUser);

            if (userRoles.Count == 0)
            {
                _logger.LogInformation("The user {Id} has no roles", appUser.Id);

                return GetScopesWithAccessStatus(scopes, _ => false);
            }

            var appRoles = await _roleManager.Roles
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) && userRoles.Contains(x.Name))
                .ToListAsync(cancellationToken);

            if (appRoles.Exists(
                    x => string.Equals(x.Name, RoleConstants.AdminRole, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogInformation("The user {Id} has {Role} role", appUser.Id, RoleConstants.AdminRole);

                return GetScopesWithAccessStatus(scopes, _ => true);
            }

            var roleClaims = new List<string>();

            foreach (var appRole in appRoles)
            {
                var claims = await _roleManager.GetClaimsAsync(appRole);

                roleClaims.AddRange(claims.Where(t => t.Type == RoleConstants.ScopeType).Select(x => x.Value));
            }

            var roleScopes = roleClaims.Distinct()
                .ToList();

            return GetScopesWithAccessStatus(
                scopes,
                scope => roleScopes.Contains(scope, StringComparer.OrdinalIgnoreCase));
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(CheckUserAccessAsync));
        }

        return new Dictionary<string, bool>();
    }

    private Dictionary<string, bool> GetScopesWithAccessStatus(
        IEnumerable<string> scopes,
        Func<string, bool> accessChecker)
    {
        var response = new Dictionary<string, bool>();

        foreach (var scope in scopes)
        {
            if (!RoleConstants.ActionRequiredPermissions.ContainsKey(scope))
            {
                _logger.LogError("Not found requested scope {Scope}", scope);

                continue;
            }

            response.Add(scope, accessChecker(scope));
        }

        return response;
    }
}
