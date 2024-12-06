using System.Text;
using System.Text.Encodings.Web;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.Models;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Email.Services;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Factories;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Repositories;
using GlobalCoders.PSP.BackendApi.Identity.Configuration;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IdentityConstants = GlobalCoders.PSP.BackendApi.Identity.Constants.IdentityConstants;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Services;

public class EmployeeService : IEmployeeService
{
    private readonly ILogger<EmployeeService> _logger;
    private readonly UserManager<EmployeeEntity> _userManager;
    private readonly IMailService _mailService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IdentityConfiguration _identityConfiguration;

    public EmployeeService(ILogger<EmployeeService> logger,
        UserManager<EmployeeEntity> userManager,
        IMailService mailService,
        IEmployeeRepository employeeRepository,
        IOptions<IdentityConfiguration> identityConfiguration)
    {
        _logger = logger;
        _userManager = userManager;
        _mailService = mailService;
        _employeeRepository = employeeRepository;
        _identityConfiguration = identityConfiguration.Value;
    }

    public async Task<EmployeeResponseModel?> GetAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        try
        {
            var appUserEntity = await _userManager.Users
                .Include(x => x.UserPermissions)
                .ThenInclude(x => x.AppRole)
                .AsNoTracking()
                .Where(x => x.Id == employeeId)
                .FirstOrDefaultAsync(cancellationToken);

            if (appUserEntity == null)
            {
                _logger.LogWarning("AppUserEntity not found by Id {Id}", employeeId);

                return null;
            }

            return EmployeeResponseModelFactory.Create(appUserEntity);
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(GetAsync));
        }

        return null;
    }

    public async Task<BasePagedResponse<EmployeeResponseListModel>> GetAllAsync(EmployeeFilter filter, CancellationToken cancellationToken)
    {
        try
        {
            var queryAppUsers = _userManager.Users;
                if(filter.OrganizationId != null && filter.OrganizationId != Guid.Empty)
                {
                    queryAppUsers = queryAppUsers.Where(x => x.MerchantId == filter.OrganizationId);
                }
                
                if(filter.IsActive != null)
                {
                    queryAppUsers = queryAppUsers.Where(x => x.IsActive == filter.IsActive);
                }
                    
                queryAppUsers = queryAppUsers
                .Include(x => x.UserPermissions)
                .ThenInclude(x => x.AppRole)
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery();

            if (!string.IsNullOrWhiteSpace(filter.SearchValue))
            {
                queryAppUsers = queryAppUsers
                    .Where(
                        x => (x.Email != null
                              && EF.Functions.ILike(x.Email, $"%{filter.SearchValue}%"))
                             || EF.Functions.ILike(x.Name, $"%{filter.SearchValue}%"));
            }

            var total = await queryAppUsers.CountAsync(cancellationToken);
            
            queryAppUsers = queryAppUsers
                .Skip((filter.Page - 1) * filter.ItemsPerPage)
                .Take(filter.ItemsPerPage);

            var appUsers = queryAppUsers.ToList();

            return BasePagedResopnseFactory.Create(appUsers.Select(x => EmployeeResponseListModelFactory.Create(x)).ToList(), filter, total);
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(GetAllAsync));
        }

        return new BasePagedResponse<EmployeeResponseListModel>();
    }

    public async Task<bool> DeleteAsync(Guid employeeId)
    {
        return await _employeeRepository.DeleteAsync(employeeId);
    }

    public async Task<ValidationDetails> CreateAsync(EmployeeCreateRequest createRequest, CancellationToken cancellationToken)
    {
        try
        {
            var user = EmployeeEntityFactory.Create(
                createRequest);

            user.PasswordHash = PasswordHelper.GetPasswordHash(user, Guid.NewGuid().ToString());

            await _userManager.SetUserNameAsync(user, createRequest.Email);

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Something was wrong. {@Errors}", result.Errors);

                return ValidationDetailsFactory.Fail(result.Errors.FirstOrDefault()?.Description ?? "Failed to create user");
            }

            if (!string.IsNullOrWhiteSpace(createRequest.Role))
            {
                await _userManager.AddToRoleAsync(user, createRequest.Role);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var routeValues = new RouteValueDictionary
            {
                [IdentityConstants.EmailQueryParam] = createRequest.Email,
                [IdentityConstants.ResetCodeQueryParam] = code
            };

            await _mailService.SendPasswordResetCodeAsync(
                createRequest.Email,
                HtmlEncoder.Default.Encode(_identityConfiguration.RedirectUrls.GetResetPasswordUrl(routeValues)),
                cancellationToken);

            return ValidationDetailsFactory.Ok();
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(CreateAsync));
        }

        return ValidationDetailsFactory.Fail("Failed to create user");
    }

    public async Task<ValidationDetails> UpdateAsync(EmployeeUpdateRequest updateRequest, CancellationToken cancellationToken)
    {
        try
        {
            var appUser = await _userManager.FindByIdAsync(updateRequest.Id.ToString());

            if (appUser == null)
            {
                _logger.LogWarning("AppUser not found by Id {Id} for update", updateRequest.Id);

                return ValidationDetailsFactory.Fail("User not found");
            }

            appUser.Email = updateRequest.Email;
            appUser.Name = updateRequest.Name;
            appUser.IsActive = updateRequest.IsActive;
            appUser.MerchantId = updateRequest.OrganizationId;

            appUser.Minute = updateRequest.Minute;
            appUser.Hour = updateRequest.Hour;
            appUser.DayMounth = updateRequest.DayOfMonth;
            appUser.Mounth = updateRequest.Month;
            appUser.DayWeek = updateRequest.DayOfWeek;

            var isUpdated = await _employeeRepository.UpdateAsync(appUser, updateRequest.Role, cancellationToken);

            if (isUpdated)
            {
                return ValidationDetailsFactory.Ok();
            }

            _logger.LogWarning("Something was wrong when try update");
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(UpdateAsync));
        }

        return ValidationDetailsFactory.Fail("Failed to update user");
    }
}