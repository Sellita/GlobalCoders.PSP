using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Services;
using GlobalCoders.PSP.BackendApi.PaymentsService.Models;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Controllers;

public class OrdersController : BaseApiController
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IOrdersService _ordersService;

    public OrdersController(ILogger<OrdersController> logger, IAuthorizationService authorizationService, IOrdersService ordersService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
        _ordersService = ordersService;
    }
    
    [HttpGet("[action]/{orderId}")]
    public async Task<ActionResult<OrderResponseModel>> Id(Guid orderId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        
        var result = await _ordersService.GetAsync(orderId);
      
        if(result == null)
        {
            return NotFound();
        }
        
        var user = await _authorizationService.GetUserAsync(User);

        if (user?.Merchant?.Id != result.MerchantId && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            
             _logger.LogWarning("User ({UserId}) has no permissions to view order {OrderId}", User.GetUserId(), orderId);

            return NotFound();
        }
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<BasePagedResponse<OrdersListModel>>> All(OrdersFilter filter, CancellationToken cancellationToken)
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
            _logger.LogWarning("User ({UserId}) has no permissions to view all orders", User.GetUserId());

            if (user == null)
            {
                return NotFound();
            }
            
            filter.MerchantId = user.MerchantId;
        }
        
        var result = await _ordersService.GetAllAsync(filter);
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(OrderCreateModel orderCreateModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        var user = await _authorizationService.GetUserAsync(User);
        
        if(orderCreateModel.MerchantId == null)
        {
            orderCreateModel.MerchantId = user?.MerchantId ?? null;
        }
        
        if(orderCreateModel.EmployeeId == null)
        {
            orderCreateModel.EmployeeId = user?.Id ?? null;
        }

        var createModel = OrderEntityFactory.Create(orderCreateModel);
        
        if((orderCreateModel.EmployeeId != user?.Id || 
           orderCreateModel.MerchantId != user?.MerchantId) && 
           !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            return Unauthorized();
        }
        
        var result = await _ordersService.CreateAsync(createModel, cancellationToken);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to update order");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(OrderUpdateModel orderUpdateModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || orderUpdateModel.Id == Guid.Empty)
        {
            return ValidationProblem();
        }
        
        var user = await _authorizationService.GetUserAsync(User);
        
        if (!(await _ordersService.HasPermissionAsync(orderUpdateModel.Id, user?.MerchantId)) && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            return Unauthorized();
        }
        
        var updateModel = OrderEntityFactory.CreateUpdate(orderUpdateModel);
        
        var (result, message) = await _ordersService.UpdateAsync(updateModel);

        if (result)
        {
            return Ok();
        }
        
        return BadRequest(message);
    }
    
    [HttpDelete("[action]/{orderId}")]
    public async Task<IActionResult> Delete(Guid orderId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var user = await _authorizationService.GetUserAsync(User);
        
        if (!(await _ordersService.HasPermissionAsync(orderId, user?.MerchantId)) && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            return Unauthorized();
        }
        
        var result = await _ordersService.DeleteAsync(orderId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete order");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> ChangeStatus(OrderChangeStatusRequestModel orderChangeStatusRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var user = await _authorizationService.GetUserAsync(User);
        
        if (!(await _ordersService.HasPermissionAsync(orderChangeStatusRequest.OrderId, user?.MerchantId)) && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            return Unauthorized();
        }
        
        var (result, message) = await _ordersService.ChangeStatusAsync(orderChangeStatusRequest);
        
        if (result)
        {
            return Ok();
        }
        
        return BadRequest(message);
    } 
    
    [HttpPost("[action]")]
    public async Task<IActionResult> ChangeProductQuantity(OrderChangeProductRequestModel orderChangeStatusRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var user = await _authorizationService.GetUserAsync(User);
        
        if (!(await _ordersService.HasPermissionAsync(orderChangeStatusRequest.OrderId, user?.MerchantId)) && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            return Unauthorized();
        }
        
        var (result, message) = await _ordersService.ChangeOrderProductAsync(orderChangeStatusRequest, cancellationToken);
        
        if (result)
        {
            return Ok();
        }
        
        return BadRequest(message);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<PaymentInfo>> MakePayment(OrderMakePaymentRequestModel orderMakePaymentRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var user = await _authorizationService.GetUserAsync(User);
        
        if (!(await _ordersService.HasPermissionAsync(orderMakePaymentRequest.OrderId, user?.MerchantId)) && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            return Unauthorized();
        }
        
        var (result, message) = await _ordersService.MakePaymentAsync(orderMakePaymentRequest, cancellationToken);
        
        if (result != null)
        {
            return Ok(result);
        }
        
        return BadRequest(message);
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult<PaymentInfo>> ResumePayment(OrderResumePaymentModel resumePaymentRequest, CancellationToken cancellationToken)
    {
        if (resumePaymentRequest.PaymentId == Guid.Empty || resumePaymentRequest.OrderId == Guid.Empty)
        {
            return BadRequest();
        }

        var (result, message) = await _ordersService.ResumePaymentAsync(resumePaymentRequest, cancellationToken);
        
        if (result != null)
        {
            return Ok(result);
        }
        
        return BadRequest(message);
    }
    
     
    [HttpPost("[action]")]
    public async Task<IActionResult> ChangeTips(TipsRequestModel tipsRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var user = await _authorizationService.GetUserAsync(User);
        
        if (!(await _ordersService.HasPermissionAsync(tipsRequest.OrderId, user?.MerchantId)) && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllOrganizations],
                cancellationToken))
        {
            return Unauthorized();
        }
        
        var (result, message) = await _ordersService.ChangeTipsAsync(tipsRequest, cancellationToken);
        
        if (result)
        {
            return Ok();
        }
        
        return BadRequest(message);
    }
}