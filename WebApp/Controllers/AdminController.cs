using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

// All admin-related pages here.

public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}