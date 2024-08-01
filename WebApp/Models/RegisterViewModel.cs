using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Please enter valid first name!")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Please enter valid last name!")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Please enter valid username!")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Please enter valid password!")]
    public string Password { get; set; }
    public int Role { get; set; } = 1;
}