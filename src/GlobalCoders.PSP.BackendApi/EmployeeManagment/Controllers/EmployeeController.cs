using GlobalCoders.PSP.BackendApi.Base.Controller;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Controllers;


public class EmployeeController : BaseApiController
{ 
    [HttpGet]
    [Route("{employeeId}")]
    public IActionResult Index(int employeeId)
    {
        return Ok("Employee Managment");
    }
}