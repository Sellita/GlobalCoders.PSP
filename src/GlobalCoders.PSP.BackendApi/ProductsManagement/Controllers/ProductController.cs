using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Factories;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Services;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Controllers;

public class ProductController : BaseApiController
{
    private readonly ILogger<ProductController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IProductService _productService;

    public ProductController(ILogger<ProductController> logger, IAuthorizationService authorizationService, IProductService productService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _productService = productService;
    }
    
    [HttpGet("[action]/{productId}")]
    public async Task<ActionResult<ProductResponseModel>> Id(Guid productId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);

        var merchant = user?.MerchantId;
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
             _logger.LogWarning("User ({UserId}) has no permissions to view all organizations", User.GetUserId());

             if (!merchant.HasValue)
             {
                 return Unauthorized();
             }
             
        }
        
        var result = await _productService.GetAsync(productId, merchant);
        
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
            
            filter.MerchantId = user.MerchantId;
            
            if(filter.MerchantId == null)
            {
                return Unauthorized();
            }
        }
        
        var result = await _productService.GetAllAsync(filter);
        
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
        
        var result = await _productService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to create product");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(ProductUpdateModel organizationUpdateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var updateModel = ProductEntityFactory.CreateUpdate(organizationUpdateModel);
        
        var result = await _productService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update Product");
    }
    
    [HttpDelete("[action]/{organizationId}")]
    public async Task<IActionResult> Delete(Guid organizationId)
    {
        var result = await _productService.DeleteAsync(organizationId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete organization");
    }
}