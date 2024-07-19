using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class LoginController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}