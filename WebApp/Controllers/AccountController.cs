using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

// Login, Register, Forbidden here

public class AccountController : Controller
{
    public IActionResult Logout()
    {
        return RedirectToAction("Index", "Home");
    }
}