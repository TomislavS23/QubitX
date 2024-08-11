using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class LogController : Controller
{
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public LogController(IApiService service, IMapper mapper)
    {
        _apiService = service;
        _mapper = mapper;
    }
    
    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        try
        {
            var model = new List<LogViewModel>();

            var count = await _apiService.GetLogCount(token);
            ViewBag.Count = count;
            
            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Manage", "Admin");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Display()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        try
        {
            var logs = await _apiService.GetLogsDefault(token);
            var count = await _apiService.GetLogCount(token);

            var model = _mapper.Map<IList<LogViewModel>>(logs);
            ViewBag.Count = count;
            
            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Manage", "Admin");
        }
    }
    
    public async Task<IActionResult> Display(int n)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        try
        {
            var logs = await _apiService.GetLogs(token, n);
            var count = await _apiService.GetLogCount(token);

            var model = _mapper.Map<IList<LogViewModel>>(logs);
            ViewBag.Count = count;
            
            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Manage", "Admin");
        }
    }
}