using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Services;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Controllers;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Controllers;

public class ReservationController : BaseApiController
{
    private readonly ILogger<ReservationController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IReservationService _reservationService;

    public ReservationController(ILogger<ReservationController> logger, IAuthorizationService authorizationService, IReservationService reservationService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _reservationService = reservationService;
    }
    
    [HttpGet("[action]/{reservationId}")]
    public async Task<ActionResult<ReservationResponseModel>> Id(Guid reservationId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        var merchantId = user?.MerchantId;
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
            _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), reservationId);

            if (!merchantId.HasValue)
            {
                Unauthorized();
            }
        }
        
        var result = await _reservationService.GetAsync(reservationId, merchantId);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
}