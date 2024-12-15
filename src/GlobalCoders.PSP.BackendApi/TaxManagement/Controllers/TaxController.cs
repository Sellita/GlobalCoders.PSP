using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.TaxManagement.Factories;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.TaxManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Controllers;

public class TaxController : BaseApiController
{
    private readonly ILogger<TaxController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly ITaxService _taxService;
    
    public TaxController(ILogger<TaxController> logger,IAuthorizationService authorizationService, ITaxService taxService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _taxService = taxService;
    }
    
    /// <summary>
    /// Get a specific tax by ID
    /// </summary>
    [HttpGet("[action]/{taxId:guid}")]
    public async Task<ActionResult<TaxResponseModel>> Id(Guid taxId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        Guid? merchantId = null;
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to get organization by id", User.GetUserId());

            var user  = await _authorizationService.GetUserAsync(User);
            
            if(user == null || user.MerchantId == null)
            {
                return NotFound();
            }
            
            merchantId = user.MerchantId;
        }
        
        var result = await _taxService.GetAsync(taxId, merchantId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    /// <summary>
    /// Get all taxes
    /// </summary>
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<TaxListModel>>> All(TaxFilter filter, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to view all taxes", User.GetUserId());

            var user  = await _authorizationService.GetUserAsync(User);
            
            if(user == null || user.MerchantId == null)
            {
                return NotFound();
            }
            
            filter.MerchantId = user.MerchantId;
        }
        
        var result = await _taxService.GetAllAsync(filter);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Create a new tax
    /// </summary>
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(TaxCreateModel taxCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var createModel = TaxEntityFactory.Create(taxCreateModel);
        
        var result = await _taxService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to create tax");
    }
    
    /// <summary>
    /// Update an existing tax
    /// </summary>
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(TaxUpdateModel taxUpdateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var updateModel = TaxEntityFactory.CreateUpdate(taxUpdateModel);
        
        var result = await _taxService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update tax");
    }
    
    /// <summary>
    /// Delete a tax
    /// </summary>
    [HttpDelete("[action]/{taxId:guid}")]
    public async Task<IActionResult> Delete(Guid taxId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        Guid? merchantId = null;

        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to view all taxes", User.GetUserId());

            var user  = await _authorizationService.GetUserAsync(User);
            
            if(user == null || user.MerchantId == null)
            {
                return NotFound();
            }
            
            merchantId = user.MerchantId;
        }
        
        var result = await _taxService.DeleteAsync(taxId, merchantId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete tax");
    }
}