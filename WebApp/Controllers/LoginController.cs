using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using WebApp.Models;

namespace WebApp.Controllers;

public class LoginController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public LoginController(IHttpClientFactory client)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient"); 
    }
    
    public IActionResult Index()
    {
        try
        {
            // Check for JWT token
            var token = User.Claims.FirstOrDefault(c => c.Type == "JWT")?.Value;
            
            if (token != null)
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

                return RedirectToAction("Index", role == "Admin" ? "Admin" : "User");
            }

            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<IActionResult> Login(string returnUrl, LoginViewModel login)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/auth/login?username={login.Username}&password={login.Password}");
            
            if (!response.IsSuccessStatusCode)
            {
                // needs fixing
                return RedirectToAction("Index", login);
            }
            
            // API Response handling
            var responseContent = await response.Content.ReadAsStringAsync();
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(responseContent);
            

            // Cookie creation
            var claims = new List<Claim>
            {
                new Claim("JWT", responseContent),
                new Claim(ClaimTypes.Name, tokenData.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value),
                new Claim(ClaimTypes.Role, tokenData.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);   
            }

            if (tokenData.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == "User")
            {
                return RedirectToAction("Index", "User");
            }
            
            return RedirectToAction("Index", "Admin");
        }
        catch (Exception e)
        {
            // TODO: Add proper error page
            return RedirectToAction("Index", "User");
        }
    }
}