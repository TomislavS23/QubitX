using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

// All admin-related pages here.

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}