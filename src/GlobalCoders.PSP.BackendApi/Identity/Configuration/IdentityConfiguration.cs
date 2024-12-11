using GlobalCoders.PSP.BackendApi.Email.Helpers;

namespace GlobalCoders.PSP.BackendApi.Identity.Configuration;

public sealed class IdentityConfiguration
{
    public const string SectionName = "Identity";

    public required string DefaultUserEmail { get; set; }

    public required RedirectUrls RedirectUrls { get; set; }

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(DefaultUserEmail)
            || !EmailHelper.IsValidateEmail(DefaultUserEmail))
        {
            return false;
        }

        return RedirectUrls.IsValid();
    }
}
