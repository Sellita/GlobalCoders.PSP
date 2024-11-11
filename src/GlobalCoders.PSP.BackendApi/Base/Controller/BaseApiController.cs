using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.Base.Controller;

//todo add in future [Authorize]
[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{

}
