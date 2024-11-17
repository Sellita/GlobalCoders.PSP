using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Factories;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Repositories;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Controllers;

public class OrganizationController : BaseApiController
{
    private readonly ILogger<OrganizationController> _logger;
    private readonly IMerchantService _merchantRepository;

    public OrganizationController(ILogger<OrganizationController> logger, IMerchantService merchantRepository)
    {
        _logger = logger;
        _merchantRepository = merchantRepository;
    }
    
    [HttpGet("[action]/{organizationId}")]
    public async Task<ActionResult<OrganizationResponseModel>> Id(Guid organizationId)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
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