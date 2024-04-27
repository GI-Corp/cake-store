using System.Security.Claims;

namespace Shared.Common.Authorization.JWT.Interfaces;

public interface IJwtFactory
{
    Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
    ClaimsIdentity GenerateClaimsIdentity(string userName, IEnumerable<Claim> claims);
}