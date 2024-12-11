using System.Reflection;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Attributes;
using GlobalCoders.PSP.BackendApi.Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.Identity.Handlers;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    private readonly ILogger<PermissionAuthorizationHandler> _logger;
    private readonly UserManager<EmployeeEntity> _userManager;
    private readonly RoleManager<PermisionTemplateEntity> _roleManager;

    public PermissionAuthorizationHandler(
        ILogger<PermissionAuthorizationHandler> logger,
        UserManager<EmployeeEntity> userManager,
        RoleManager<PermisionTemplateEntity> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        var appUser = await GetUserAsync(context);

        if (appUser == null)
        {
            _logger.LogWarning("Invalid user token");

            return;
        }

        if (!IsProtectedAction(context))
        {
            context.Succeed(requirement);

            return;
        }

        if (await IsAdminAsync(appUser))
        {
            context.Succeed(requirement);

            return;
        }

        if (await HasPermissionAsync(context, appUser))
        {
            context.Succeed(requirement);
        }
    }

    private static bool IsProtectedAction(AuthorizationHandlerContext context)
    {
        if (context.Resource is not HttpContext httpContext)
        {
            return true;
        }

        var descriptor = httpContext
            .GetEndpoint()
            ?.Metadata
            .GetMetadata<ControllerActionDescriptor>();

        return descriptor?.MethodInfo.GetCustomAttribute<AllowAnyAccessAttribute>() == null;
    }

    private async Task<EmployeeEntity?> GetUserAsync(AuthorizationHandlerContext context)
    {
        return await _userManager.GetUserAsync(context.User);
    }

    private async Task<bool> IsAdminAsync(EmployeeEntity appUser)
    {
        return await _userManager.IsInRoleAsync(appUser, RoleConstants.AdminRole);
    }

    private async Task<bool> HasPermissionAsync(AuthorizationHandlerContext context, EmployeeEntity appUser)
    {
        var actionId = GetActionId(context);

        if (string.IsNullOrWhiteSpace(actionId))
        {
            _logger.LogError("{Service} ActionId is empty", GetType().Name);

            return false;
        }

        var userAppRoles = await _userManager.GetRolesAsync(appUser);

        var appRoles = await GetRelevantRolesAsync(userAppRoles);

        foreach (var appRole in appRoles)
        {
            var claims = await _roleManager.GetClaimsAsync(appRole);

            if (claims.Any(t => t.Type == RoleConstants.ScopeType && t.Value == actionId))
            {
                return true;
            }
        }

        return false;
    }

    private static string GetActionId(AuthorizationHandlerContext context)
    {
        if (context.Resource is not HttpContext httpContext)
        {
            return string.Empty;
        }

        var controllerActionDescriptor = httpContext
            .GetEndpoint()
            ?.Metadata
            .GetMetadata<ControllerActionDescriptor>();

        if (controllerActionDescriptor == null)
        {
            return string.Empty;
        }

        var controllerName = controllerActionDescriptor.ControllerName;
        var actionName = controllerActionDescriptor.ActionName;

        return $"{controllerName}:{actionName}".ToLower();
    }

    private async Task<List<PermisionTemplateEntity>> GetRelevantRolesAsync(IEnumerable<string> userAppRoles)
    {
        return await _roleManager.Roles
            .Where(x => !string.IsNullOrWhiteSpace(x.Name) && userAppRoles.Contains(x.Name))
            .ToListAsync();
    }
}
