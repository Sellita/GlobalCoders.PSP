using GlobalCoders.PSP.BackendApi.Base.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.Identity.Constants;

public static class RoleConstants
{
    public const string AdminRole = "Admin";
    public const string ScopeType = "Scope";

    public static readonly IReadOnlyDictionary<string, bool> ActionRequiredPermissions =
        ControllerHelper.GetActionRequiredPermissions<ControllerBase>();
}
