using System.Text.RegularExpressions;

namespace GlobalCoders.PSP.BackendApi.Email.Helpers;

public static class EmailHelper
{
    public const string HtmlType = "text/html";
    public const string TextPlainType = "text/plain";

    private const string EmailValidationRegEx = @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$";
    public static bool IsValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var regex = new Regex(EmailValidationRegEx);

            var match = regex.Match(email);

            return match.Success;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsHtml(string type)
    {
        return string.Equals(HtmlType, type, StringComparison.OrdinalIgnoreCase);
    }
}
