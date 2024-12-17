using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Factories;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Controllers;

public class OrganizationController : BaseApiController//todo we need to check access
{
    private readonly ILogger<OrganizationController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMerchantService _merchantService;

    public OrganizationController(ILogger<OrganizationController> logger, IAuthorizationService authorizationService, IMerchantService merchantService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _merchantService = merchantService;
    }
    
    [HttpGet("[action]/{organizationId}")]
    public async Task<ActionResult<OrganizationResponseModel>> Id(Guid organizationId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
 
        var user = await _authorizationService.GetUserAsync(User);
        
        if (user?.MerchantId != organizationId && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
             _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), organizationId);

            return NotFound();
        }
        
        var result = await _merchantService.GetAsync(organizationId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<OrganizationsListModel>>> All(OrganizationsFilter filter, CancellationToken cancellationToken)
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
            _logger.LogWarning("User ({UserId}) has no permissions to view all organization", User.GetUserId());

            if (user == null || !user.MerchantId.HasValue)
            {
                return NotFound();
            }
            
            var userOrganization = await _merchantService.GetAsync(user.MerchantId.Value);

            if (userOrganization == null)
            {
                return NotFound();
            }

            return Ok(BasePagedResopnseFactory.CreateSingle(OrganizationsListModelFactory.Create(userOrganization)));
        }
        
        var result = await _merchantService.GetAllAsync(filter);
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(OrganizationCreateModel organizationCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var createModel = MerchantEntityFactory.Create(organizationCreateModel);
        
        var result = await _merchantService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update organization");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(OrganizationUpdateModel organizationUpdateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var updateModel = MerchantEntityFactory.CreateUpdate(organizationUpdateModel);
        
        var result = await _merchantService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update organization");
    }
    
    [HttpDelete("[action]/{organizationId}")]
    public async Task<IActionResult> Delete(Guid organizationId)
    {
        var result = await _merchantService.DeleteAsync(organizationId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete organization");
    }
}