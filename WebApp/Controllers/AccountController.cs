using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.DataTransferObjects;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public AccountController(IHttpClientFactory client, IApiService service, IMapper mapper)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
        _apiService = service;
        _mapper = mapper;
    }

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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string returnUrl, LoginViewModel model)
    {
        try
        {
            var response = await _apiService.LoginAsync(model.Username, model.Password);
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(response);
            
            // Cookie creation
            var claims = new List<Claim>
            {
                new Claim("JWT", response),
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
            
            HttpContext.Response.Cookies.Append("JWT", response, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

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
    
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        try
        {
            // API Response handling
            var response = await _apiService.RegisterAsync(_mapper.Map<RegisterDTO>(model));
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(response);
            

            // Cookie creation
            var claims = new List<Claim>
            {
                new Claim("JWT", response),
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
            
            HttpContext.Response.Cookies.Append("JWT", response, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            
            return RedirectToAction("Index", "User");
        }
        catch (Exception e)
        {
            // TODO: Add proper error page
            return RedirectToAction("Index", "User");
        }
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
}