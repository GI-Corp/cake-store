using Identity.Domain.Entities.Token;

namespace Identity.Application.Dto.Auth;

public class TokenDto
{
    public Guid SessionId { get; set; }
    public JwtToken Token { get; set; }
}