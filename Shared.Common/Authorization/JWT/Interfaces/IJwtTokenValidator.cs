using System.Security.Claims;

namespace Shared.Common.Authorization.JWT.Interfaces;

public interface IJwtTokenValidator
{
    ClaimsPrincipal? GetPrincipalFromToken(string token);
}