using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Dto.Auth;

public class RegisterDto
{
    [Required]
    [MaxLength(30)]
    public virtual string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required] 
    [MaxLength(3)] 
    public string LanguageId { get; set; }
    
    [Required] 
    [MaxLength(30)] 
    public string FirstName { get; set; }
    
    [Required] 
    [MaxLength(30)] 
    public string LastName { get; set; }

    [Required] 
    [MaxLength(25)] 
    public string Role { get; set; } = "Customer";
}