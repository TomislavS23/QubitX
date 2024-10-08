using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebAPI.Models;
using WebAPI.Security;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth/")]
public class LoginController : Controller
{
    private readonly QubitXContext _context;
    private readonly AuthenticationUtils _authenticationUtils;

    public LoginController(QubitXContext context, IConfiguration configuration)
    {
        _context = context;
        _authenticationUtils = new AuthenticationUtils(configuration, context);
    }
    
    [HttpGet("login")]
    public IActionResult Login(string username, string password)
    {
        try
        {
            var user = _authenticationUtils.Authenticate(username, password);

            if (user != null)
            {
                var token = _authenticationUtils.GenerateToken(user);
                return Ok(token);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDTO register)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Username = register.Username,
                HashedPassword = HashUtils.ComputeSHA256Hash(register.Password),
                IdRole = register.Role
            };

            var query = _context.Users.Add(user);

            _context.SaveChanges();
            
            var token = _authenticationUtils.GenerateToken(user);
            return Ok(token);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("changepassword")]
    [Authorize(Roles = "Admin, User")]
    public ActionResult ChangePassword(string username, string password)
    {
        try
        {
            var query = _context.Users.FirstOrDefault(u => u.Username == username);
            query.HashedPassword = HashUtils.ComputeSHA256Hash(password);

            _context.SaveChanges();

            return Ok("Password changed successfully.");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}