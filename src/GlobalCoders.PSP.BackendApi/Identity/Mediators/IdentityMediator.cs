using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GlobalCoders.PSP.BackendApi.Identity.Mediators;

public sealed class IdentityMediator : IIdentityMediator
{
    private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions;
    private readonly UserManager<EmployeeEntity> _userManager;
    private readonly SignInManager<EmployeeEntity> _signInManager;

    public IdentityErrorDescriber ErrorDescriber => _userManager.ErrorDescriber;

    public IdentityMediator(
        IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
        UserManager<EmployeeEntity> userManager,
        SignInManager<EmployeeEntity> signInManager)
    {
        _bearerTokenOptions = bearerTokenOptions;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public void SetAuthenticationScheme(string authenticationScheme)
    {
        _signInManager.AuthenticationScheme = authenticationScheme;
    }

    public async Task<IdentityResult> SetUserNameAsync(EmployeeEntity user, string userName)
    {
        return await _userManager.SetUserNameAsync(user, userName);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(EmployeeEntity user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(EmployeeEntity user)
    {
        return await _signInManager.CreateUserPrincipalAsync(user);
    }

    public async Task<EmployeeEntity?> ValidateSecurityStampAsync(ClaimsPrincipal principal)
    {
        return await _signInManager.ValidateSecurityStampAsync(principal);
    }

    public AuthenticationTicket? GetRefreshTicket(string refreshToken, string authenticationScheme)
    {
        var refreshTokenProtector =
            _bearerTokenOptions.Get(authenticationScheme).RefreshTokenProtector;

        return refreshTokenProtector.Unprotect(refreshToken);
    }

    public async Task<EmployeeEntity?> FindUserByIdAsync(Guid userId)
    {
        return await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<EmployeeEntity?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(EmployeeEntity user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user, token);
    }

    public async Task<bool> IsEmailConfirmedAsync(EmployeeEntity user)
    {
        return await _userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<IdentityResult> ChangeEmailAsync(EmployeeEntity user, string newEmail, string token)
    {
        return await _userManager.ChangeEmailAsync(user, newEmail, token);
    }

    public async Task<IdentityResult> ResetPasswordAsync(EmployeeEntity user, string token, string newPassword)
    {
        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<string> GetUserIdAsync(EmployeeEntity user)
    {
        return await _userManager.GetUserIdAsync(user);
    }

    public async Task<string> GenerateChangeEmailTokenAsync(EmployeeEntity user, string newEmail)
    {
        return await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(EmployeeEntity user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<SignInResult> PasswordSignInAsync(
        string email,
        string password,
        bool isPersistent,
        bool lockoutOnFailure)
    {
        return await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
    }
}
