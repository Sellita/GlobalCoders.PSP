namespace GlobalCoders.PSP.BackendApi.Identity.Constants;

public static class IdentityConstants
{
    public const int MaximumEmailLength = 50;
    public const int PasswordRequiredLength = 8;

    public const string CodeQueryParam = "code";
    public const string EmailQueryParam = "email";
    public const string ResetCodeQueryParam = "resetCode";
    public const string ChangedEmailQueryParam = "changedemail";
    public const string UserIdQueryParam = "userId";

    public const string PasswordResetSubjectText = "Reset Password";
    public const string PasswordResetContentMessage = "Click here to reset your password";

    public const string EmailConfirmationSubjectText = "Confirm Email";
    public const string EmailConfirmationContentMessage = "Click here to confirm your email";

    public const string HtmlAnchorTemplate = "<a href=\"{0}\">\"{1}\"</a>";
}
