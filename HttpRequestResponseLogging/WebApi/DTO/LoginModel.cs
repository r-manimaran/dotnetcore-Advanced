using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO;

public class LoginModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
