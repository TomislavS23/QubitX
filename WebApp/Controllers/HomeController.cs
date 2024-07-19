using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory client)
    {
        _logger = logger;
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public IActionResult HelloWorld()
    {
        ViewBag["Miro"] = "miric";
        
        return View();
    }
}