using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.Models;
using GlobalCoders.PSP.BackendApi.Email.Helpers;

namespace GlobalCoders.PSP.BackendApi.Email.Models;

public sealed class MailMessageModel 
{
    public string From { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string Replay { get; set; } = string.Empty;
    public string ReplayName { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string ToName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsHtmlMessage { get; set; }

    public ValidationDetails Validate()
    {
        if (!EmailHelper.IsValidateEmail(To))
        {
            return ValidationDetailsFactory.Fail($"Address {nameof(To)} format invalid");
        }

        if (!EmailHelper.IsValidateEmail(Replay))
        {
            return ValidationDetailsFactory.Fail($"Address {nameof(Replay)} format invalid");
        }

        if (!EmailHelper.IsValidateEmail(From))
        {
            return ValidationDetailsFactory.Fail($"Address {nameof(From)} format invalid");
        }

        return ValidationDetailsFactory.Ok();
    }
}
