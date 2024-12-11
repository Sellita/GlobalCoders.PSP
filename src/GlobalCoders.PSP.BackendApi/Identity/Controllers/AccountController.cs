using System.Net.Mime;
using System.Text;
using System.Text.Encodings.Web;
using GlobalCoders.PSP.BackendApi.Base.Constants;
using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Email.Services;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Attributes;
using GlobalCoders.PSP.BackendApi.Identity.Configuration;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Mediators;
using GlobalCoders.PSP.BackendApi.Identity.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ExternalIdentityConstants = Microsoft.AspNetCore.Identity.IdentityConstants;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;
using IdentityConstants = GlobalCoders.PSP.BackendApi.Identity.Constants.IdentityConstants;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest;

namespace GlobalCoders.PSP.BackendApi.Identity.Controllers;

public sealed class AccountController : BaseApiController
{
    private readonly ILogger<AccountController> _logger;
    private readonly IdentityConfiguration _identityConfiguration;
    private readonly IIdentityMediator _identityMediator;
    private readonly UserManager<EmployeeEntity> _userManager;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMailService _mailService;


    public AccountController(
        ILogger<AccountController> logger,
        IOptions<IdentityConfiguration> identityConfiguration,
        IIdentityMediator identityMediator,
        UserManager<EmployeeEntity> userManager,
        IAuthorizationService authorizationService,
        IMailService mailService)
    {
        _logger = logger;
        _identityConfiguration = identityConfiguration.Value;
        _identityMediator = identityMediator;
        _userManager = userManager;
        _authorizationService = authorizationService;
        _mailService = mailService;
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Consumes(typeof(LoginRequest), MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccessTokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> LoginAsync(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("{Model} invalid", nameof(LoginRequest));

            return ValidationProblem(title: ErrorsMessageConstants.OneOrMoreValidationErrors);
        }

        _identityMediator.SetAuthenticationScheme(ExternalIdentityConstants.BearerScheme);

        var result = await _identityMediator.PasswordSignInAsync(request.Email, request.Password, false, true);

        if (result.Succeeded)
        {
            return Empty;
        }

        _logger.LogWarning("Fail to login {Login}", request.Email);

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Consumes(typeof(RefreshRequest), MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccessTokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> RefreshTokenAsync(RefreshRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            _logger.LogWarning("Refresh token is empty");

            return ValidationProblem(title: "Invalid refresh token");
        }

        var refreshTicket = _identityMediator.GetRefreshTicket(
            request.RefreshToken,
            ExternalIdentityConstants.BearerScheme);

        if (refreshTicket?.Properties.ExpiresUtc is not { } expiresUtc
            || DateTimeOffset.UtcNow >= expiresUtc
            || await _identityMediator.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            _logger.LogWarning("Refresh token expired {RefreshToken}", request.RefreshToken);

            return Challenge();
        }

        var newPrincipal = await _identityMediator.CreateUserPrincipalAsync(user);

        return SignIn(newPrincipal, ExternalIdentityConstants.BearerScheme);
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Consumes(typeof(ConfirmEmailRequest), MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("{Model} invalid {@Request}", nameof(ConfirmEmailRequest), request);

            return ValidationProblem(title: ErrorsMessageConstants.OneOrMoreValidationErrors);
        }

        if (await _identityMediator.FindUserByIdAsync(request.UserId) is not { } user)
        {
            _logger.LogWarning("User not found by id {UserId}", request.UserId);

            return BadRequest();
        }

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException formatException)
        {
            _logger.LogExceptionError(formatException, nameof(ConfirmEmailAsync));

            return BadRequest();
        }

        IdentityResult result;

        if (string.IsNullOrWhiteSpace(request.ChangedEmail))
        {
            result = await _identityMediator.ConfirmEmailAsync(user, code);
        }
        else
        {
            result = await _identityMediator.ChangeEmailAsync(user, request.ChangedEmail, code);

            if (result.Succeeded)
            {
                result = await _identityMediator.SetUserNameAsync(user, request.ChangedEmail);
            }
        }

        if (result.Succeeded)
        {
            return Ok();
        }

        _logger.LogWarning("Confirm email result was not success: {@Result}", result);

        return BadRequest();
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Consumes(typeof(ResendConfirmationEmailRequest), MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> ResendConfirmationEmail(ResendConfirmationEmailRequest request)
    {
        if (await _identityMediator.FindByEmailAsync(request.Email) is not { } user)
        {
            return Ok();
        }

        await SendConfirmationEmailAsync(user, request.Email, false);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Consumes(typeof(ResetPasswordRequest), MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _identityMediator.FindByEmailAsync(request.Email);

        if (user is null || !await _identityMediator.IsEmailConfirmedAsync(user))
        {
            _logger.LogWarning("User not found by email {Email} or email is not confirmed", request.Email);

            foreach (var identityError in IdentityResult.Failed(_identityMediator.ErrorDescriber.InvalidToken()).Errors)
            {
                ModelState.AddModelError(identityError.Code, identityError.Description);
            }

            return ValidationProblem(title: ErrorsMessageConstants.OneOrMoreValidationErrors);
        }

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetCode));

        var result = await _identityMediator.ResetPasswordAsync(user, code, request.NewPassword);

        if (result.Succeeded)
        {
            return Ok();
        }

        _logger.LogWarning("Result ({@Result}). Request({@Request})", result, request);

        foreach (var identityError in result.Errors)
        {
            ModelState.AddModelError(identityError.Code, identityError.Description);
        }

        return ValidationProblem(title: ErrorsMessageConstants.OneOrMoreValidationErrors);
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Consumes(typeof(ForgotPasswordRequest), MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<ActionResult> ForgotPasswordAsync(
        ForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _identityMediator.FindByEmailAsync(request.Email);

        if (user is null || !await _identityMediator.IsEmailConfirmedAsync(user))
        {
            return Ok();
        }

        var code = await _identityMediator.GeneratePasswordResetTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var routeValues = new RouteValueDictionary
        {
            [IdentityConstants.EmailQueryParam] = request.Email,
            [IdentityConstants.ResetCodeQueryParam] = code
        };

        await _mailService.SendPasswordResetCodeAsync(
            request.Email,
            HtmlEncoder.Default.Encode(_identityConfiguration.RedirectUrls.GetResetPasswordUrl(routeValues)),
            cancellationToken);

        return Ok();
    }

    private async Task SendConfirmationEmailAsync(
        EmployeeEntity user,
        string email,
        bool isEmailChange)
    {
        var code = isEmailChange
            ? await _identityMediator.GenerateChangeEmailTokenAsync(user, email)
            : await _identityMediator.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await _identityMediator.GetUserIdAsync(user);

        var routeValues = new RouteValueDictionary
        {
            [IdentityConstants.UserIdQueryParam] = userId,
            [IdentityConstants.CodeQueryParam] = code
        };

        if (isEmailChange)
        {
            routeValues.Add(IdentityConstants.ChangedEmailQueryParam, email);
        }

        await _mailService.SendConfirmationEmailAsync(
            email,
            HtmlEncoder.Default.Encode(_identityConfiguration.RedirectUrls.GetConfirmationEmailUrl(routeValues)),
            CancellationToken.None);
    }
    
    [AllowAnyAccess]
    [HttpPost("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Consumes(typeof(ChangePasswordRequest), MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<Dictionary<string, bool>>> CheckAccessAsync(
        List<string> scopes,
        CancellationToken cancellationToken)
    {
        if (scopes.Exists(string.IsNullOrWhiteSpace))
        {
            _logger.LogWarning("Invalid request data {@Data}", scopes);

            return ValidationProblem(title: ErrorsMessageConstants.OneOrMoreValidationErrors);
        }

        var appUser = await _userManager.GetUserAsync(User);

        if (appUser != null)
        {
            return await _authorizationService.CheckUserAccessAsync(appUser, scopes, cancellationToken);
        }

        _logger.LogWarning("Invalid user token");

        return Unauthorized();
    }
}
