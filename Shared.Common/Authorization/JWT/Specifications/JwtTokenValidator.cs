using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.Authorization.JWT.Interfaces;
using Shared.Common.Logging.Interfaces;

namespace Shared.Common.Authorization.JWT.Specifications;

public class JwtTokenValidator : IJwtTokenValidator
{
    private readonly IEventLoggerService<JwtTokenValidator> _eventLogger;
    private readonly JwtIssuerOptions _jwtOptions;
    private readonly TokenValidationParameters _validationParameters;

    public JwtTokenValidator(
        IOptions<JwtIssuerOptions> jwtOptions,
        IOptions<TokenValidationParameters> validationParameters,
        IEventLoggerService<JwtTokenValidator> eventLogger)
    {
        _jwtOptions = jwtOptions.Value ??
                      throw new ArgumentNullException(nameof(jwtOptions));

        _validationParameters = validationParameters.Value ??
                                throw new ArgumentNullException(nameof(validationParameters));

        _eventLogger = eventLogger ??
                       throw new ArgumentNullException(nameof(eventLogger));
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = _validationParameters;

        tokenValidationParameters.ValidateLifetime = false;

        tokenValidationParameters.IssuerSigningKey = _jwtOptions.SigningCredentials.Key as SymmetricSecurityKey;
        tokenValidationParameters.ValidIssuer = _jwtOptions.Issuer;
        tokenValidationParameters.ValidateAudience = false;

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (
                !(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase)
            )
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        catch (Exception exception)
        {
            _eventLogger.LogError($"Token validation error {exception.Message}");
        }

        return null;
    }
}