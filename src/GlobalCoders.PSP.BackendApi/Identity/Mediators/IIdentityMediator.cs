using System.Security.Claims;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace GlobalCoders.PSP.BackendApi.Identity.Mediators;

public interface IIdentityMediator
{
    IdentityErrorDescriber ErrorDescriber { get; }

    void SetAuthenticationScheme(string authenticationScheme);

    Task<IdentityResult> SetUserNameAsync(EmployeeEntity user, string userName);

    Task<string> GeneratePasswordResetTokenAsync(EmployeeEntity user);

    Task<ClaimsPrincipal> CreateUserPrincipalAsync(EmployeeEntity user);

    Task<EmployeeEntity?> ValidateSecurityStampAsync(ClaimsPrincipal principal);

    AuthenticationTicket? GetRefreshTicket(string refreshToken, string authenticationScheme);

    Task<EmployeeEntity?> FindUserByIdAsync(Guid userId);

    Task<EmployeeEntity?> FindByEmailAsync(string email);

    Task<IdentityResult> ConfirmEmailAsync(EmployeeEntity user, string token);

    Task<bool> IsEmailConfirmedAsync(EmployeeEntity user);

    Task<IdentityResult> ChangeEmailAsync(EmployeeEntity user, string newEmail, string token);

    Task<IdentityResult> ResetPasswordAsync(EmployeeEntity user, string token, string newPassword);

    Task<string> GetUserIdAsync(EmployeeEntity user);

    Task<string> GenerateChangeEmailTokenAsync(EmployeeEntity user, string newEmail);

    Task<string> GenerateEmailConfirmationTokenAsync(EmployeeEntity user);

    Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
}
