using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Enum;
using GlobalCoders.PSP.BackendApi.Identity.Services;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Extensions
{
    public static class DiscountAuthorizationExtension
    {
        public static async Task<bool> HasDiscountManagementPermissionsAsync(this IAuthorizationService authorizationService, ClaimsPrincipal user)
        {
            return await authorizationService.HasPermissionsAsync(user, permissions: [Permissions.CanManageDiscounts]);
        }
    }
}

