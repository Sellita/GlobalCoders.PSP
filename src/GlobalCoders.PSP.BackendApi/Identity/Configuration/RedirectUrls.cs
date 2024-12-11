namespace GlobalCoders.PSP.BackendApi.Identity.Configuration;

public sealed class RedirectUrls
{
    public required string BaseRedirectUrl { get; set; }

    public required string ResetPasswordRedirectUrl { get; set; }
    public required string ConfirmationEmailRedirectUrl { get; set; }

    public bool IsValid()
    {
        if (!Uri.IsWellFormedUriString(BaseRedirectUrl, UriKind.Absolute))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(ResetPasswordRedirectUrl))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(ConfirmationEmailRedirectUrl))
        {
            return false;
        }

        return true;
    }
}
