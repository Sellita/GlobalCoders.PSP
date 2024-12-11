using GlobalCoders.PSP.BackendApi.Base.Helpers;
using GlobalCoders.PSP.BackendApi.Identity.Configuration;



namespace GlobalCoders.PSP.BackendApi.Identity.Extensions;

public static class RedirectUrlsExtension
{
    public static string GetResetPasswordUrl(
        this RedirectUrls redirectUrls,
        RouteValueDictionary? routeValueDictionary)
    {
        return UrlHelper.BuildUrl(
            redirectUrls.BaseRedirectUrl,
            redirectUrls.ResetPasswordRedirectUrl,
            routeValueDictionary);
    }

    public static string GetConfirmationEmailUrl(
        this RedirectUrls redirectUrls,
        RouteValueDictionary? routeValueDictionary)
    {
        return UrlHelper.BuildUrl(
            redirectUrls.BaseRedirectUrl,
            redirectUrls.ConfirmationEmailRedirectUrl,
            routeValueDictionary);
    }
}
