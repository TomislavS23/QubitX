using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers;

public class RegisterController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public RegisterController(IHttpClientFactory client)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient"); 
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Register(RegisterViewModel register)
    {
        try
        {
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register", content);
            
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
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
            
            return RedirectToAction("Index", "User");
        }
        catch (Exception e)
        {
            // TODO: Add proper error page
            return RedirectToAction("Index", "User");
        }
    }
}