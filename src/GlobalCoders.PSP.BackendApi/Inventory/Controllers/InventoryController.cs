using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Inventory.ModelsDto;
using GlobalCoders.PSP.BackendApi.Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;

namespace GlobalCoders.PSP.BackendApi.Inventory.Controllers;

public class InventoryController : BaseApiController
{
    private readonly ILogger<InventoryController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IInventoryService _inventoryService;

    public InventoryController(ILogger<InventoryController> logger, IAuthorizationService authorizationService, IInventoryService inventoryService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _inventoryService = inventoryService;
    }
    
    [HttpGet("[action]/{organizationId}/{productId}")]
    public async Task<ActionResult<decimal>> Quantity(
        Guid organizationId,
        Guid productId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        
        if (user?.Merchant?.Id != organizationId && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
             _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), organizationId);

            return NotFound();
        }
        
        var result = await _inventoryService.GetQuantityAsync(organizationId, productId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<decimal>> Add(InventoryQuantityChangeRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        
        if (user?.Merchant?.Id != request.OrganizationId && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
            _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), request.OrganizationId);

            return Unauthorized();
        }
        
        var (result, quantity) = await _inventoryService.ChangeQuantityAsync(request.OrganizationId, request.ProductId, request.QuantityChange, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }
        
        return Ok(quantity);
    }
}