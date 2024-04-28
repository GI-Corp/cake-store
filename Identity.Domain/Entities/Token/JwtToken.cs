namespace Identity.Domain.Entities.Auth;

public class JwtToken
{
    public string Login { get; set; }
    public string AuthToken { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}