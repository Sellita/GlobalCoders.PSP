using GlobalCoders.PSP.BackendApi.Base.Constants;
using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Factories;
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
        Guid? merchantId = null;
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
            _logger.LogWarning("User ({UserId}) has no permissions to view organization {OrganizaitonId}", User.GetUserId(), reservationId);
            merchantId = user?.MerchantId;

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
    
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<ReservationListModel>>> All(ReservationFilter filter, CancellationToken cancellationToken)
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
            filter.MerchantId = user?.MerchantId;

            if (!filter.MerchantId.HasValue)
            {
                Unauthorized();
            }
        }
        
        var result = await _reservationService.GetAllAsync(filter);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(ReservationCreateModel reservationCreateModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        var user = await _authorizationService.GetUserAsync(User);
        var serviceUser = await _authorizationService.GetUserByIdAsync(reservationCreateModel.EmployeeId);

        if (serviceUser == null)
        {
            return BadRequest(ErrorsMessageConstants.EmployeeNotFound);
        }

        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to create service not his merchant", User.GetUserId());
            
            if (!serviceUser.MerchantId.HasValue)
            {
                return BadRequest();
            }
            
            if (user == null || user.Merchant == null)
            {
                return NotFound();
            }
            
            if(user.MerchantId != serviceUser.MerchantId)
            {
                return Unauthorized();
            }
        }
        
        var (result, message) = await _reservationService.CreateAsync(reservationCreateModel, serviceUser);

        if (result)
        {
            return Ok();
        }
        
        return Problem(message);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<List<TimeSlot>>> GetEmptyTimeSlots(TimeSlotRequest request, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid || request.EmployeeId == Guid.Empty)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserByIdAsync(request.EmployeeId);
        
        if(user == null)
        {
            return BadRequest(ErrorsMessageConstants.EmployeeNotFound);
        }
        
        var result = await _reservationService.GetTimeSlotsAsync(request, user);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<List<TimeSlot>>> Cancel(ReservationCancelRequest request, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid || request.ReservationId == Guid.Empty)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        Guid? merchantId = null;
        if (!await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            _logger.LogWarning("User ({UserId}) has no permissions to create service not his merchant", User.GetUserId());
           
            if (user == null || user.Merchant == null)
            {
                return NotFound();
            }
            
            merchantId = user.MerchantId;
            
            
            if(!merchantId.HasValue)
            {
                return Unauthorized();
            }
        }
        
        var (result, message) = await _reservationService.CancelAppointment(request, merchantId);
        
        if(!result)
        {
            return NotFound(message);
        }
        
        return Ok(result);
    }
    
}