using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Configuration;
using GlobalCoders.PSP.BackendApi.Identity.Constants;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GlobalCoders.PSP.BackendApi.Identity.Services.Initialization;

public sealed class RoleSetupService : IRoleSetupService
{
    private const string AdminRoleName = RoleConstants.AdminRole;
    private const string ScopeType = RoleConstants.ScopeType;

    private readonly ILogger<RoleSetupService> _logger;
    private readonly RolesConfiguration _rolesConfiguration;
    private readonly RoleManager<PermisionTemplateEntity> _roleManager;
    private readonly UserManager<EmployeeEntity> _userManager;

    public RoleSetupService(
        ILogger<RoleSetupService> logger,
        IOptions<RolesConfiguration> roleOptions,
        RoleManager<PermisionTemplateEntity> roleManager,
        UserManager<EmployeeEntity> userManager)
    {
        _logger = logger;
        _rolesConfiguration = roleOptions.Value;
        _roleManager = roleManager;
        _userManager = userManager;

        ValidateRoleClaims();
    }

    public async Task<bool> RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Synchronizing roles and roles claims...");

            await EnsureAdminRoleExistsAsync();

            var appRoles = await _roleManager.Roles.ToListAsync(cancellationToken);

            foreach (var appRole in appRoles)
            {
                if (string.IsNullOrWhiteSpace(appRole.Name))
                {
                    _logger.LogWarning("Removed invalid role {@Role}. Reason: Empty role name", appRole);

                    await _roleManager.DeleteAsync(appRole);

                    continue;
                }

                if (!_rolesConfiguration.ContainsKey(appRole.Name) && appRole.Name != AdminRoleName)
                {
                    await RemoveRoleFromUsersAsync(appRole.Name);

                    await _roleManager.DeleteAsync(appRole);

                    _logger.LogWarning("Role {Role} deleted. Reason: Role not found in configuration", appRole.Name);

                    continue;
                }

                await SynchronizeRoleClaimsAsync(appRole);
            }

            await AddNewRolesAsync(
                _rolesConfiguration
                    .Where(x => !appRoles.Select(a => a.Name).Contains(x.Key)));

            _logger.LogInformation("Synchronize roles and roles claims is done");

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(RoleSetupService));
        }

        return false;
    }

    private async Task AddNewRolesAsync(IEnumerable<KeyValuePair<string, HashSet<string>>> roles)
    {
        foreach (var (role, claims) in roles)
        {
            var appRole = AppRoleModelsFactory.CreateAppRole(role);

            await _roleManager.CreateAsync(appRole);

            _logger.LogInformation("Added new role {RoleName}", appRole.Name);

            foreach (var claim in claims)
            {
                await _roleManager.AddClaimAsync(appRole, new Claim(RoleConstants.ScopeType, claim.ToLower()));

                _logger.LogInformation("Added claim {Claim} to role {Role}", claim.ToLower(), appRole.Name);
            }
        }
    }

    private void ValidateRoleClaims()
    {
        foreach (var scope in _rolesConfiguration.SelectMany(x => x.Value).Distinct())
        {
            if (!RoleConstants.ActionRequiredPermissions.ContainsKey(scope.ToLower())
                && !Enum.GetNames<Permissions>().Contains(scope, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Invalid scope {scope} in {nameof(_rolesConfiguration)}");
            }
        }
    }

    private async Task EnsureAdminRoleExistsAsync()
    {
        var adminRole = await _roleManager.FindByNameAsync(AdminRoleName);

        if (adminRole == null)
        {
            await _roleManager.CreateAsync(AppRoleModelsFactory.CreateAppRole(AdminRoleName));

            _logger.LogInformation("Created role {Role}", AdminRoleName);
        }
    }

    private async Task SynchronizeRoleClaimsAsync(PermisionTemplateEntity role)
    {
        if (string.IsNullOrWhiteSpace(role.Name))
        {
            _logger.LogError("Role name is empty. {@Role}", role);

            return;
        }

        if (role.Name == AdminRoleName)
        {
            return;
        }

        var claims = GetClaimsForRole(role.Name);

        var existingClaims = await _roleManager.GetClaimsAsync(role);

        foreach (var claim in existingClaims.Where(
                     claim => !claims.Exists(c => c.Type == claim.Type && c.Value == claim.Value)))
        {
            await _roleManager.RemoveClaimAsync(role, claim);

            _logger.LogWarning("Removed claim {Claim} from role {Role}", claim, role.Name);
        }

        foreach (var claim in claims.Where(
                     claim => !existingClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value)))
        {
            await _roleManager.AddClaimAsync(role, claim);

            _logger.LogInformation("Append role {Role} with claim {Claim}", role.Name, claim);
        }
    }

    private List<Claim> GetClaimsForRole(string roleName)
    {
        return _rolesConfiguration.TryGetValue(roleName, out var roleClaims)
            ? roleClaims.Select(claim => new Claim(ScopeType, claim.ToLower())).ToList()
            : [];
    }

    private async Task RemoveRoleFromUsersAsync(string roleName)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

        foreach (var user in usersInRole)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                continue;
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError(
                    "Error occurred while removing role for user {UserId}: {Error}",
                    user.Id,
                    error.Description);
            }
        }
    }
}
