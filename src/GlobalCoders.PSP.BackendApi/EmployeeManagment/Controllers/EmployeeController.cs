using GlobalCoders.PSP.BackendApi.Base.Controller;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Controllers;


public class EmployeeController : BaseApiController
{ 
    
    [HttpGet("[action]/{employeeId}")]
    public IActionResult Id(int employeeId)
    {
        throw new NotImplementedException();
    }  
    
    [HttpGet("[action]/{organizationId}")]
    public IActionResult Organization(int organizationId)
    {
        throw new NotImplementedException();
    }  
    
    [HttpGet("[action]/{isActive}")]
    public IActionResult Active(bool isActive)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("[action]/{employeeId}")]
    public IActionResult Delete(int employeeId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("[action]")]
    public IActionResult Create()
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("[action]")]
    public IActionResult Update()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("[action]")]
    public IActionResult All()
    {
        throw new NotImplementedException();
    }
}