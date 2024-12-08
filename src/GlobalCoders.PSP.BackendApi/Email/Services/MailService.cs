using System.Globalization;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Email.Configuration;
using GlobalCoders.PSP.BackendApi.Email.Factories;
using GlobalCoders.PSP.BackendApi.Email.Helpers;
using GlobalCoders.PSP.BackendApi.Email.Models;
using GlobalCoders.PSP.BackendApi.Identity.Constants;
using Microsoft.Extensions.Options;

namespace GlobalCoders.PSP.BackendApi.Email.Services;

public sealed class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly IMailProvider _mailMessageSendService;

    public MailService(
        ILogger<MailService> logger,
        IMailProvider mailMessageSendService)
    {
        _logger = logger;
        _mailMessageSendService = mailMessageSendService;
    }

    public async Task<bool> SendAsync(MailMessageModel mailMessageModel, CancellationToken cancellationToken)
    {
        try
        {
            var validationDetails = mailMessageModel.Validate();

            if (!validationDetails.Success)
            {
                _logger.LogError(
                    "Mail message model invalid. {@MailMessageModel}. {Error}",
                    mailMessageModel,
                    validationDetails.ErrorMessage);

                return false;
            }

            return await _mailMessageSendService.SendAsync(mailMessageModel, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(SendAsync));
        }

        return false;
    }

    public async Task<bool> SendPasswordResetCodeAsync(
        string email,
        string redirectUrl,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute))
            {
                _logger.LogError("Password reset request {Param} is invalid. {Url}", nameof(redirectUrl), redirectUrl);

                return false;
            }

            if (!EmailHelper.IsValidateEmail(email))
            {
                _logger.LogError(
                    "Password reset request {Param} is invalid. {Email}",
                    nameof(email),
                    email);

                return false;
            }

            var mailMessageEntity = MailFactory.CreateMailMessage(
                IdentityConstants.PasswordResetSubjectText,
                string.Format(
                    IdentityConstants.HtmlAnchorTemplate,
                    redirectUrl,
                    IdentityConstants.PasswordResetContentMessage),
                email,
                true);

            return await _mailMessageSendService.SendAsync(mailMessageEntity, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(SendPasswordResetCodeAsync));
        }

        return false;
    }

    public async Task<bool> SendConfirmationEmailAsync(
        string email,
        string redirectUrl,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute))
            {
                _logger.LogError("Password reset request {Param} is invalid. {Url}", nameof(redirectUrl), redirectUrl);

                return false;
            }

            if (!EmailHelper.IsValidateEmail(email))
            {
                _logger.LogError(
                    "Confirmation email request {Param} is invalid. {Email}",
                    nameof(email),
                    email);

                return false;
            }

            var mailMessageEntity = MailFactory.CreateMailMessage(
                    IdentityConstants.EmailConfirmationSubjectText,
                    string.Format(
                        IdentityConstants.HtmlAnchorTemplate,
                        redirectUrl,
                        IdentityConstants.EmailConfirmationContentMessage), 
                    email,
                    true);

            return await _mailMessageSendService.SendAsync(mailMessageEntity, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(SendConfirmationEmailAsync));
        }

        return false;
    }
}
