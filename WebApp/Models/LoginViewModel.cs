using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter valid username!")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Please enter valid password!")]
    public string Password { get; set; }
}