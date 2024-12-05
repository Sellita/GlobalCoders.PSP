using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Factories;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Controllers;

public class SurchargeController : BaseApiController
{
    private readonly ILogger<SurchargeController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly ISurchargeService _surchargeService;

    public SurchargeController(ILogger<SurchargeController> logger,IAuthorizationService authorizationService, ISurchargeService surchargeService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _surchargeService = surchargeService;
    }

    /// <summary>
    /// Get a specific surcharge by ID
    /// </summary>
    [HttpGet("[action]/{surchargeId:guid}")]
    public async Task<ActionResult<SurchargeResponseModel>> Id(Guid surchargeId, CancellationToken cancellationToken)
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
        
        var result = await _surchargeService.GetAsync(surchargeId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Get all surcharges
    /// </summary>
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<SurchargeListModel>>> All(SurchargeFilter filter)
    {
        if(!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var result = await _surchargeService.GetAllAsync(filter);
        
        return Ok(result);
    }

    /// <summary>
    /// Create a new surcharge
    /// </summary>
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(SurchargeCreateModel surchargeCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var createModel = SurchargeEntityFactory.Create(surchargeCreateModel);
        
        var result = await _surchargeService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update surcharge");
    }

    /// <summary>
    /// Update an existing surcharge
    /// </summary>
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(SurchargeUpdateModel surchargeUpdateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var updateModel = SurchargeEntityFactory.CreateUpdate(surchargeUpdateModel);
        
        var result = await _surchargeService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update surcharge");
    }

    /// <summary>
    /// Delete a surcharge
    /// </summary>
    [HttpDelete("[action]/{surchargeId:guid}")]
    public async Task<IActionResult> Delete(Guid surchargeId)
    {
        var result = await _surchargeService.DeleteAsync(surchargeId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete surcharge");
    }
}
