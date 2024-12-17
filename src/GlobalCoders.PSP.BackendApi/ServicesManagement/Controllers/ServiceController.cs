using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Factories;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Services;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Controllers;

public class ServiceController : BaseApiController
{
    private readonly ILogger<ServiceController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IServicesService _servicesService;

    public ServiceController(ILogger<ServiceController> logger, IAuthorizationService authorizationService, IServicesService servicesService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _servicesService = servicesService;
    }
    
    [HttpGet("[action]/{serviceId}")]
    public async Task<ActionResult<ServiceResponseModel>> Id(Guid serviceId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        var merchantId = user?.Merchant?.Id;
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
             _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), serviceId);

            return NotFound();
        }
        
        var result = await _servicesService.GetAsync(serviceId, merchantId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<ServiceListModel>>> All(ServiceFilter filter, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var user = await _authorizationService.GetUserAsync(User);
        
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to view all services", User.GetUserId());

            if (user == null || user.Merchant == null)
            {
                return NotFound();
            }
            
            filter.MerchantId = user.MerchantId;
            
            if(filter.MerchantId == null)
            {
                return Unauthorized();
            }
        }
        
        var result = await _servicesService.GetAllAsync(filter);
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(ServiceCreateModel organizationCreateModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        var user = await _authorizationService.GetUserAsync(User);

        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to create service not his merchant", User.GetUserId());

            if (user == null || user.Merchant == null)
            {
                return NotFound();
            }
            
            if(user.Merchant.Id != organizationCreateModel.EmployeeId)
            {
                return Unauthorized();
            }
        }
        
        var createModel = ServiceEntityFactory.Create(organizationCreateModel);
        
        var result = await _servicesService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to create service");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(ServiceUpdateModel organizationUpdateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var updateModel = ServiceEntityFactory.CreateUpdate(organizationUpdateModel);
        
        var result = await _servicesService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update Service");
    }
    
    [HttpDelete("[action]/{organizationId}")]
    public async Task<IActionResult> Delete(Guid organizationId)
    {
        var result = await _servicesService.DeleteAsync(organizationId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete organization");
    }
}