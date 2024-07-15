using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Json.Utilities;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models;

namespace WebAPI.Security;

public class AuthenticationUtils
{
    private readonly IConfiguration _configuration;
    private readonly QubitXContext _context;

    public AuthenticationUtils(IConfiguration configuration, QubitXContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    
    public string GenerateToken(User user)
    {
        var query =
            _context.Users
                .Include(r => r.IdRoleNavigation)
                .Where(u => u.HashedPassword == user.HashedPassword)
                .Select(r => r.IdRoleNavigation.RoleType)
                .FirstOrDefault();
            
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Username),
            new Claim(ClaimTypes.Role, query)
        };
        
        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public User Authenticate(string username, string password)
    {
        var hashedPassword = HashUtils.ComputeSHA256Hash(password);
        
        var query = _context.Users.FirstOrDefault(u =>
            u.Username == username && u.HashedPassword == hashedPassword);

        if (query != null)
        {
            return query;
        }
        return null;
    }
}