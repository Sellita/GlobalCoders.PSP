using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Base.Helpers;
using GlobalCoders.PSP.BackendApi.Email.Services;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Configuration;
using GlobalCoders.PSP.BackendApi.Identity.Constants;
using GlobalCoders.PSP.BackendApi.Identity.Factories;
using GlobalCoders.PSP.BackendApi.Identity.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using IdentityConstants = GlobalCoders.PSP.BackendApi.Identity.Constants.IdentityConstants;

namespace GlobalCoders.PSP.BackendApi.Identity.Services.Initialization;

public sealed class DefaultUserSetupService : IDefaultUserSetupService
{
    private readonly ILogger<DefaultUserSetupService> _logger;
    private readonly IdentityConfiguration _identityConfiguration;
    private readonly UserManager<EmployeeEntity> _userManager;
    private readonly IMailService _mailService;

    public DefaultUserSetupService(
        ILogger<DefaultUserSetupService> logger,
        IOptions<IdentityConfiguration> identityConfiguration,
        UserManager<EmployeeEntity> userManager,
        IMailService mailService)
    {
        _logger = logger;
        _identityConfiguration = identityConfiguration.Value;
        _userManager = userManager;
        _mailService = mailService;
    }

    public async Task<bool> RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_userManager.Users.Any())
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(_identityConfiguration.DefaultUserEmail))
            {
                _logger.LogError("Config {Property} is empty", nameof(_identityConfiguration.DefaultUserEmail));

                return false;
            }

            var appUser = AppUserModelsFactory.Create(
                _identityConfiguration.DefaultUserEmail,
                true);

            appUser.PasswordHash = PasswordHelper.GetPasswordHash(appUser, Guid.NewGuid().ToString());

            _logger.LogInformation("Try creating default user {User}...", _identityConfiguration.DefaultUserEmail);

            var result = await _userManager.CreateAsync(appUser);

            if (result is not {Succeeded: true})
            {
                _logger.LogError("Something was wrong when try create default admin. Result {@Result}", result);

                return false;
            }

            await _userManager.UpdateSecurityStampAsync(appUser);

            await _userManager.AddToRoleAsync(appUser, RoleConstants.AdminRole);

            _logger.LogInformation("Success added default user {Email}", appUser.Email);

            var code = await _userManager.GeneratePasswordResetTokenAsync(appUser);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Try to send password reset mail to default user {Email}", appUser.Email);

            var routeValues = new RouteValueDictionary
            {
                [IdentityConstants.EmailQueryParam] = appUser.Email,
                [IdentityConstants.ResetCodeQueryParam] = code
            };

            var passwordResetUrl = UrlHelper.BuildUrl(
                _identityConfiguration.RedirectUrls.BaseRedirectUrl,
                _identityConfiguration.RedirectUrls.ResetPasswordRedirectUrl,
                routeValues);

            var encodedPasswordResetUrl = HtmlEncoder.Default.Encode(passwordResetUrl);

            _logger.LogInformation("Reset url: {Url}", passwordResetUrl);

            return await _mailService.SendPasswordResetCodeAsync(
                _identityConfiguration.DefaultUserEmail,
                encodedPasswordResetUrl,
                cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(DefaultUserSetupService));
        }

        return false;
    }
}
