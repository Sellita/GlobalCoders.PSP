using System.Globalization;
using GlobalCoders.PSP.BackendApi.Email.Models;

namespace GlobalCoders.PSP.BackendApi.Email.Services;

public interface IMailService
{
    Task<bool> SendAsync(MailMessageModel mailMessageModel, CancellationToken cancellationToken);

    Task<bool> SendPasswordResetCodeAsync(
        string email,
        string redirectUrl,
        CancellationToken cancellationToken);

    Task<bool> SendConfirmationEmailAsync(string email, string redirectUrl, CancellationToken cancellationToken);
}
