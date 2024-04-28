using Identity.Application.Dto.Auth;
using Identity.Application.Dto.Identity;
using Identity.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Controllers;
using Shared.Presentation.ViewModels.Exceptions;

namespace Identity.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity")]
[AllowAnonymous]
public class RegistrationController : BaseAuthController
{
    private IIdentityService _identityService;
    
    public RegistrationController(IIdentityService identityService)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }
    
    /// <summary>
    ///     Register a new user by username and password
    /// </summary>
    /// <param name="registrationModel"></param>
    /// <response code="200">User model</response>
    /// <response code="400">User already exists or registration failed</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppUserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseViewModel))]
    [HttpPost("register")]
    public async Task<ActionResult<AppUserDto>> RegisterUserAsync([FromBody] RegisterDto registrationModel)
    {
        var userDto = await _identityService.CreateOrUpdateUserAsync(registrationModel);
        return Ok(userDto);

    }
}