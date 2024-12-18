using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Factories;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Services;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Controllers;

public class DiscountController : BaseApiController
{
    private readonly ILogger<DiscountController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IDiscountService _discountService;
    
    public DiscountController(ILogger<DiscountController> logger,IAuthorizationService authorizationService, IDiscountService discountService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _discountService = discountService;
    }
    
    /// <summary>
    /// Get a specific discountId by ID
    /// </summary>
    [HttpGet("[action]/{discountId:guid}")]
    public async Task<ActionResult<DiscountResponseModel>> Id(Guid discountId, CancellationToken cancellationToken)
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
        
        var result = await _discountService.GetAsync(discountId, merchantId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    /// <summary>
    /// Get all discountIdes
    /// </summary>
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<DiscountListModel>>> All(DiscountFilter filter, CancellationToken cancellationToken)
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
            _logger.LogWarning("User ({UserId}) has no permissions to view all discounts", User.GetUserId());

            var user  = await _authorizationService.GetUserAsync(User);
            
            if(user == null || user.MerchantId == null)
            {
                return NotFound();
            }
            
            filter.MerchantId = user.MerchantId;
        }
        
        var result = await _discountService.GetAllAsync(filter);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Create a new discount
    /// </summary>
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(DiscountCreateModel discountCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var createModel = DiscountEntityFactory.Create(discountCreateModel);
        
        var result = await _discountService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to create discount");
    }
    
    /// <summary>
    /// Update an existing discount
    /// </summary>
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(DiscountUpdateModel discountUpdateModel)
    {
        if (!ModelState.IsValid || discountUpdateModel.Id == Guid.Empty)
        {
            return ValidationProblem();
        }
        
        var updateModel = DiscountEntityFactory.CreateUpdate(discountUpdateModel);
        
        var result = await _discountService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update discount");
    }
    
    /// <summary>
    /// Delete a discounts
    /// </summary>
    [HttpDelete("[action]/{discountId:guid}")]
    public async Task<IActionResult> Delete(Guid discountId, CancellationToken cancellationToken)
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
            _logger.LogWarning("User ({UserId}) has no permissions to view all discounts", User.GetUserId());

            var user  = await _authorizationService.GetUserAsync(User);
            
            if(user == null || user.MerchantId == null)
            {
                return NotFound();
            }
            
            merchantId = user.MerchantId;
        }
        
        var result = await _discountService.DeleteAsync(discountId, merchantId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete discountId");
    }
}