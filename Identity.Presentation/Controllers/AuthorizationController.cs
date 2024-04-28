using Identity.Application.Dto.Identity;
using Identity.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Controllers;
using Shared.Common.Helpers;
using Shared.Presentation.ViewModels.Exceptions;

namespace Identity.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity")]
[Authorize(AuthenticationSchemes = Constants.Miscellaneous.Bearer)]
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
    [HttpPost("me")]
    public async Task<ActionResult<AppUserDto>> GetMeAsync()
    {
        var userDto = await _identityService.GetUserDetailsByIdAsync(User.UserId);
        return Ok(userDto);
    }
}