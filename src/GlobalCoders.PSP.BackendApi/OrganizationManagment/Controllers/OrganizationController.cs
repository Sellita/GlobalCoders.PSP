using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Factories;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Repositories;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Controllers;

public class OrganizationController : BaseApiController//todo we need to check access
{
    private readonly ILogger<OrganizationController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IMerchantService _merchantRepository;

    public OrganizationController(ILogger<OrganizationController> logger, IAuthorizationService authorizationService, IMerchantService merchantRepository)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _merchantRepository = merchantRepository;
    }
    
    [HttpGet("[action]/{organizationId}")]
    public async Task<ActionResult<OrganizationResponseModel>> Id(Guid organizationId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to create confirm hashTag", User.GetUserId());

            return NotFound();
        }
        
        var result = await _merchantRepository.GetAsync(organizationId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<OrganizationsListModel>>> All(OrganizationsFilter filter)
    {
        if(!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var result = await _merchantRepository.GetAllAsync(filter);
        
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
        
        var result = await _merchantRepository.CreateAsync(createModel);

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
        
        var result = await _merchantRepository.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update organization");
    }
    
    [HttpDelete("[action]/{organizationId}")]
    public async Task<IActionResult> Delete(Guid organizationId)
    {
        var result = await _merchantRepository.DeleteAsync(organizationId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete organization");
    }
}