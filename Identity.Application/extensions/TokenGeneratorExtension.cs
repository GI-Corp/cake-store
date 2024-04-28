using System.Security.Claims;
using Identity.Domain.Entities.Auth;
using Identity.Domain.Entities.Token;
using Newtonsoft.Json;
using Shared.Common.Authorization.JWT;
using Shared.Common.Authorization.JWT.Interfaces;

namespace Identity.Application.Extensions;

public static class TokenGeneratorExtension
{
    public static async Task<JwtToken> GenerateJwtToken(ClaimsIdentity identity, IJwtFactory jwtFactory,
        string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
    {
        var response = new JwtToken
        {
            Login = identity.Claims.Single(c => c.Type == "UserName").Value,
            AuthToken = await jwtFactory.GenerateEncodedToken(userName, identity),
            ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds
        };

        return response;
    }   
}