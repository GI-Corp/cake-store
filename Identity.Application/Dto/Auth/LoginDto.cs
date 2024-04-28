using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Dto.Auth;

public class LoginDto
{
    [Required]
    [MaxLength(30)]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}