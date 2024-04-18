using System.Security.Claims;
using System.Security.Principal;

namespace Shared.Common.Authorization.Claims;

public class CommonClaimsPrincipal : ClaimsPrincipal
{
    private readonly ClaimsIdentity? _claimsIdentity;

    public CommonClaimsPrincipal(IPrincipal principal) : base(principal)
    {
        _claimsIdentity = (principal as ClaimsPrincipal)?.Identity as ClaimsIdentity;
    }

    private string GetClaimValue(string claimType) => _claimsIdentity?.FindFirst(claimType)?.Value!;

    public Guid? UserId => TryParseGuid(GetClaimValue(Helpers.Constants.JwtClaimIdentifiers.Id));
    public string LanguageId => GetClaimValue(Helpers.Constants.JwtClaimIdentifiers.LanguageId);
    public string UserName => GetClaimValue(Helpers.Constants.JwtClaimIdentifiers.UserName);
    public string PhoneNumber => GetClaimValue(Helpers.Constants.JwtClaimIdentifiers.PhoneNumber);
    public Guid? SessionId => TryParseGuid(GetClaimValue(Helpers.Constants.JwtClaimIdentifiers.SessionId));

    private Guid? TryParseGuid(string value)
    {
        return Guid.TryParse(value, out Guid result) ? result : null;
    }
}
