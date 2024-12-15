using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.Controllers;

public class OrganizationStatisticController : BaseApiController
{
    private readonly ILogger<OrganizationStatisticController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IOrganizationStatisticService _organizationStatisticService;

    public OrganizationStatisticController(ILogger<OrganizationStatisticController> logger, IAuthorizationService authorizationService, IOrganizationStatisticService organizationStatisticService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _organizationStatisticService = organizationStatisticService;
    }
    
    [HttpGet("[action]")]
    public async Task<ActionResult<OrganizationDailyStatisticModel>> GetDailyStatistic(OrganizationStatisticRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        
        if (user?.Merchant.Id != request.OrganizationId && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
            _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), request.OrganizationId);

            return NotFound();
        }
        
        var result = await _organizationStatisticService.GetAsync(request);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
}