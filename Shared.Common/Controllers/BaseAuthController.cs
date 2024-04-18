using Microsoft.AspNetCore.Mvc;
using Shared.Common.Authorization.Claims;

namespace Shared.Common.Controllers;

[ApiController]
[Produces("application/json")]
public class BaseAuthController : ControllerBase
{
    private CommonClaimsPrincipal _commonClaimsPrincipal;

    public new CommonClaimsPrincipal User
    {
        get
        {
            if (_commonClaimsPrincipal != null)
                return _commonClaimsPrincipal;

            return new CommonClaimsPrincipal(HttpContext.User);
        }
    }
}