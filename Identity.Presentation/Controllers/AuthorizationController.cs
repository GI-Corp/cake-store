using Identity.Application.Dto.Auth;
using Identity.Application.Dto.Identity;
using Identity.Application.Interfaces;
using Identity.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Controllers;
using Shared.Common.Helpers;
using Shared.Presentation.ViewModels.Exceptions;

namespace Identity.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity")]
public class AuthorizationController : BaseAuthController
{
    private IIdentityService _identityService;
    
    public AuthorizationController(IIdentityService identityService)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }
    
    /// <summary>
    ///     Get a user details by JWT token
    /// </summary>
    /// <response code="200">User details model</response>
    /// <response code="400">Failed to get user details</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppUserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseViewModel))]
    [Authorize(AuthenticationSchemes = Constants.Miscellaneous.Bearer)]
    [HttpGet("me")]
    public async Task<ActionResult<AppUserDto>> GetMeAsync()
    {
        var userDto = await _identityService.GetUserDetailsByIdAsync(User.UserId);
        return Ok(userDto);
    }

    /// <summary>
    ///     Get a jwt authorization token by username and password
    /// </summary>
    /// <param name="model"></param>
    /// <response code="200">Jwt bearer token</response>
    /// <response code="400">Failed to login</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseViewModel))]
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ErrorResponseViewModel>> UserLoginAsync([FromBody] LoginDto model)
    {
        var hostAddress = Request.GetHostAddress();
        var token = await _identityService.AuthorizeUserAsync(model, hostAddress);
        return Ok(token);
    }
}