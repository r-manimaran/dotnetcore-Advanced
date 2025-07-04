using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class LoginRequestModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
