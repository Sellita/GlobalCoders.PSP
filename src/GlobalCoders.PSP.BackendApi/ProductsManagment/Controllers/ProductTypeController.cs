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

public class ProductTypeController : BaseApiController
{
    private readonly ILogger<ProductTypeController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IProductTypeService _produyctTypeService;

    public ProductTypeController(ILogger<ProductTypeController> logger, IAuthorizationService authorizationService, IProductTypeService produyctTypeService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _produyctTypeService = produyctTypeService;
    }
    
    [HttpGet("[action]/{organizationId}")]
    public async Task<ActionResult<ProductTypeResponseModel>> Id(Guid organizationId,
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
    public async Task<ActionResult<BasePagedResponse<ProductTypeListModel>>> All(ProductTypeFilter filter, CancellationToken cancellationToken)
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

            if (user == null)
            {
                return NotFound();
            }
            
            var userOrganization = await _produyctTypeService.GetAsync(user.Merchant.Id);

            if (userOrganization == null)
            {
                return NotFound();
            }

            return Ok(BasePagedResopnseFactory.CreateSingle(ProductTypeListModelFactory.Create(userOrganization)));
        }
        
        var result = await _produyctTypeService.GetAllAsync(filter);
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(ProductTypeCreateModel organizationCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var createModel = ProductTypeEntityFactory.Create(organizationCreateModel);
        
        var result = await _produyctTypeService.CreateAsync(createModel);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update organization");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(ProductTypeUpdateModel organizationUpdateModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var updateModel = ProductTypeEntityFactory.CreateUpdate(organizationUpdateModel);
        
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