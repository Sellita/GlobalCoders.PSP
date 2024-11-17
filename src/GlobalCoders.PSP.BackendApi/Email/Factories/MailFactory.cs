using GlobalCoders.PSP.BackendApi.Email.Models;

namespace GlobalCoders.PSP.BackendApi.Email.Factories;

public static class MailFactory
{
    public static MailMessageModel CreateMailMessage(
        string subject,
        string content,
        bool isHtmlMessage)
    {
        return new MailMessageModel
        {
            Subject = subject,
            Content = content,
            IsHtmlMessage = isHtmlMessage
        };
    }
}
