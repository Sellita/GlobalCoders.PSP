using System.Net;
using System.Net.Mail;
using GlobalCoders.PSP.BackendApi.Email.Configuration;
using GlobalCoders.PSP.BackendApi.Email.Models;
using Microsoft.Extensions.Options;

namespace GlobalCoders.PSP.BackendApi.Email.Services;

public sealed class SmtpMailProvider : IMailProvider, IDisposable
{
    private readonly ILogger<SmtpMailProvider> _logger;
    private readonly SmtpConfiguration _smtpConfiguration;

    public SmtpMailProvider(
        ILogger<SmtpMailProvider> logger,
        IOptions<SmtpConfiguration> options)
    {
        _logger = logger;
        _smtpConfiguration = options.Value;
    }

    public async Task<bool> SendAsync(MailMessageModel mailMessageModel, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending {@Mail}", mailMessageModel);

        try
        {
            using var smtpClient = new SmtpClient(_smtpConfiguration.SmtpServer);

            smtpClient.Port = _smtpConfiguration.Port;
            smtpClient.EnableSsl = _smtpConfiguration.EnableSsl;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Timeout = _smtpConfiguration.TimeoutInSeconds;

            if (!string.IsNullOrEmpty(_smtpConfiguration.Password))
            {
                smtpClient.Credentials = new NetworkCredential(
                    _smtpConfiguration.Username,
                    _smtpConfiguration.Password);
            }
            else
            {
                smtpClient.UseDefaultCredentials = true;
            }

            var emmailToSend = new MailMessage
            {
                From = new MailAddress(mailMessageModel.From),
                Subject = mailMessageModel.Subject,
                Body = mailMessageModel.Content,
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(emmailToSend, cancellationToken);

            _logger.LogInformation("Send completed {@Mail}", mailMessageModel);

            return true;
        }
        catch (SmtpException smtpException)
        {
            _logger.LogError(smtpException, "SMTP error occured in {Method}", nameof(SendAsync));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Method}", nameof(SendAsync));
        }

        return false;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static void Dispose(bool disposing)
    {
        if (!disposing)
        {
            //cleanup
        }
    }

    ~SmtpMailProvider()
    {
        Dispose(false);
    }
}
