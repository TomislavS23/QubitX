using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public HomeController(IHttpClientFactory client)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
    }

    public IActionResult Index()
    {
        return View();
    }
}