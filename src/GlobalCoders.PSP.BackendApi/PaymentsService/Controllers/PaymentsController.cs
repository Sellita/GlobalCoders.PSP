using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Identity.Configuration;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GlobalCoders.PSP.BackendApi.PaymentsService.Controllers;

public class PaymentsController : BaseApiController
{

    private readonly ILogger<PaymentsController> _logger;
    private readonly IOrdersService _orderService;

    private readonly string _baseUrl;
    public PaymentsController(ILogger<PaymentsController> logger, IOrdersService orderService, IOptions<IdentityConfiguration> identityConf)
    {
        _logger = logger;
        _orderService = orderService;
        _baseUrl = identityConf.Value.RedirectUrls.BaseRedirectUrl;
    }

    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<ActionResult> Success(Guid orderId, string sessionId)
    {
        _logger.LogInformation("Received success payment for order: {OrderId}", orderId);
        if (orderId == Guid.Empty || string.IsNullOrWhiteSpace(sessionId))
        {
            return BadRequest();
        }
        
        var result = await _orderService.ConfirmPaymentAsync(orderId, sessionId);


        if (!result)
        {
            return BadRequest();
        }
        
        return Redirect($"{_baseUrl}/success");
    }

    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<ActionResult> Cancel(Guid orderId, string sessionId)
    {
        _logger.LogInformation("Received Cancel payment for order: {OrderId}", orderId);

        if (orderId == Guid.Empty || string.IsNullOrWhiteSpace(sessionId))
        {
            return BadRequest();
        }
        
        var result = await _orderService.CancelPaymentAsync(orderId, sessionId);
        
        if (!result)
        {
            return BadRequest();
        }
        
        return Redirect($"{_baseUrl}/cancel");
    }
}