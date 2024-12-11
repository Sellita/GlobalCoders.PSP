using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Factories;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Services;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Controllers;

public class ProductController : BaseApiController
{
    private readonly ILogger<ProductController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IProductService _produyctTypeService;

    public ProductController(ILogger<ProductController> logger, IAuthorizationService authorizationService, IProductService produyctTypeService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _produyctTypeService = produyctTypeService;
    }
    
    [HttpGet("[action]/{organizationId}")]
    public async Task<ActionResult<ProductResponseModel>> Id(Guid organizationId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        
        if (user?.Merchant.Id != organizationId && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
             _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), organizationId);

            return NotFound();
        }
        
        var result = await _produyctTypeService.GetAsync(organizationId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<ProductListModel>>> All(ProductFilter filter, CancellationToken cancellationToken)
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
            _logger.LogWarning("User ({UserId}) has no permissions to view all products", User.GetUserId());

            if (user == null || user.Merchant == null)
            {
                return NotFound();
            }
            
            var userOrganization = await _produyctTypeService.GetAsync(user.Merchant.Id);
            
            filter.MerchantId = userOrganization?.MerchantId;
            
            if(filter.MerchantId == null)
            {
                return Unauthorized();
            }
        }
        
        var result = await _produyctTypeService.GetAllAsync(filter);
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(ProductCreateModel organizationCreateModel, CancellationToken cancellationToken)
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
            _logger.LogWarning("User ({UserId}) has no permissions to create product not his merchant", User.GetUserId());

            if (user == null || user.Merchant == null)
            {
                return NotFound();
            }
            
            if(user.Merchant.Id != organizationCreateModel.MerchantId)
            {
                return Unauthorized();
            }
        }
        
        var createModel = ProductEntityFactory.Create(organizationCreateModel);
        
        var result = await _produyctTypeService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update organization");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(ProductUpdateModel organizationUpdateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var updateModel = ProductEntityFactory.CreateUpdate(organizationUpdateModel);
        
        var result = await _produyctTypeService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update organization");
    }
    
    [HttpDelete("[action]/{organizationId}")]
    public async Task<IActionResult> Delete(Guid organizationId)
    {
        var result = await _produyctTypeService.DeleteAsync(organizationId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete organization");
    }
}