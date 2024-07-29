using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class UserController : Controller
{
    // GET => /MainPage
    [Route("/main")]
    public IActionResult Index()
    {
        return View();
    }
}