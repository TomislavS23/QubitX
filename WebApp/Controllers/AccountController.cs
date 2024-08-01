using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public AccountController(IHttpClientFactory client)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient"); 
    }

    public async Task<IActionResult> Logout()
    {
        try
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception e)
        {
            // TODO: Add proper error page
            return RedirectToAction("Index", "User");
        }

        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult Forbidden()
    {
        return RedirectToAction("Index", "Home");
    }
    
    //
    // For some reason code below works but layouts for login and register forms
    // won't render. Console throws error 404 for every css file in layout used
    // by those views. If each of views has its own controller where ex.:  
    // public IActionResult Index() is present instead of public IActionResult Login()
    // it will render layout and work properly, but if code is done like bellow,
    // in one controller, it will show blank pages without styles. Issue is
    // temporarily resolved by separating login and register in separate controllers. 
    //
    
    /*
    public IActionResult Login()
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
    
    [HttpPost]
    public async Task<IActionResult> Login(string returnUrl, LoginViewModel login)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/auth/login?username={login.Username}&password={login.Password}");
            
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

            if (returnUrl != null)
                return LocalRedirect(returnUrl);
            else
                return RedirectToAction("Index", "User");
        }
        catch (Exception e)
        {
            // TODO: Add proper error page
            return RedirectToAction("Index", "User");
        }
    }
    
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel register)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"api/auth/login?" +
                $"firstName={register.FirstName}" +
                $"&lastName={register.LastName}" +
                $"&username={register.Username}" +
                $"&password={register.Password}");
            
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
    */
}