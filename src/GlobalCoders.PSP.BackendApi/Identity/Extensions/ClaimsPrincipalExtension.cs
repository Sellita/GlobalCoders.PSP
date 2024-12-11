using System.Security.Claims;

namespace GlobalCoders.PSP.BackendApi.Identity.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims
                   .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                   ?.Value
               ?? string.Empty;
    }
}
