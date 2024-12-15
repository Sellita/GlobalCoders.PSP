using System.Net.Mime;
using GlobalCoders.PSP.BackendApi.Base.Controller;
using GlobalCoders.PSP.BackendApi.Identity.Attributes;
using GlobalCoders.PSP.BackendApi.Identity.Models;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.Identity.Controllers;

public class ProfileController  : BaseApiController
{
    private readonly IAuthorizationService _authorizationService;

    public ProfileController(
        IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    
    [AllowAnyAccess]
    [HttpGet("[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ProfileUserInfo>> GetInfoAsync()
    {
        var user = await _authorizationService.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        return ProfileUserInfoModelsFactory.CreateProfileUserInfo(user);
    }
}