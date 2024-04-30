using Shared.Common.Authorization.Claims;
using Shared.Common.Helpers;

namespace Shared.Common.Extensions.Request;

public static class HeaderExtensions
{
    public static string GetDefaultLanguageId(CommonClaimsPrincipal claims)
    {
        return claims.LanguageId;
    }
}