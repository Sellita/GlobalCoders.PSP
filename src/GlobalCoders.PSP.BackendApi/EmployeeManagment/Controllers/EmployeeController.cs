using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Services;
using GlobalCoders.PSP.BackendApi.Identity.Enums;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Controllers;


public class EmployeeController : BaseApiController
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IEmployeeService _employeeService;
    private readonly IAuthorizationService _authorizationService;

    public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, IAuthorizationService authorizationService)
    {
        _logger = logger;
        _employeeService = employeeService;
        _authorizationService = authorizationService;
    }
    
    [HttpGet("[action]/{employeeId}")]
    public async Task<ActionResult<EmployeeResponseModel?>> Id(Guid employeeId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var user = await _authorizationService.GetUserAsync(User);
        
        if (user?.Id != employeeId && !await _authorizationService.HasPermissionsAsync(
                User,
                [Permissions.CanViewAllEmployees],
                cancellationToken))
        {
            
            _logger.LogWarning("User ({UserId}) has no permissions to view employee {EmployeeId}", User.GetUserId(), employeeId);

            return NotFound();
        }
        
        var result = await _employeeService.GetAsync(employeeId, cancellationToken);
        
        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }  
    
    [HttpPost("[action]")]
    public async Task<IActionResult> All(EmployeeFilter filter, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var result = await _employeeService.GetAllAsync(filter, cancellationToken);
        
        return Ok(result);
    }  
    
    [HttpDelete("[action]/{employeeId}")]
    public async Task<IActionResult> Delete(Guid employeeId)
    {
        var result = await _employeeService.DeleteAsync(employeeId);

        if (result)
        {
            return Ok();
        }
        
        return Problem("Failed to delete employee");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(EmployeeCreateRequest createRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var result = await _employeeService.CreateAsync(createRequest, cancellationToken);

        if (result.Success)
        {
            return Ok();
        }
        
        return Problem($"Failed to update employee: {result.ErrorMessage}");
    }
    
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(EmployeeUpdateRequest updateRequest, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }
        
        var result = await _employeeService.UpdateAsync(updateRequest, cancellationToken);

        if (result.Success)
        {
            return Ok();
        }
        
        return Problem($"Failed to update employee: {result.ErrorMessage}");
    }
}