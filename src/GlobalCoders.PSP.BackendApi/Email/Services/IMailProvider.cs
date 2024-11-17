using GlobalCoders.PSP.BackendApi.Email.Models;

namespace GlobalCoders.PSP.BackendApi.Email.Services;

public interface IMailProvider
{
    Task<bool> SendAsync(MailMessageModel mailMessageModel, CancellationToken cancellationToken);
}
