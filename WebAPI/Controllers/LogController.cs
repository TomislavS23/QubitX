using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/logs")]
public class LogController : Controller
{
    private readonly QubitXContext _context;

    public LogController(QubitXContext context)
    {
        _context = context;
    }
    
    [HttpGet("get/{n}"), Authorize]
    [Authorize(Roles = "Admin")]
    public ActionResult<IList<Log>> Get(int n = 10)
    {
        try
        {
            var query = _context.Logs.Take(n).ToList();
            return Ok(query);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("count")]
    [Authorize(Roles = "Admin")]
    public ActionResult<int> GetLogCount()
    {
        try
        {
            var logNumber = _context.Logs.Count();
            return Ok(logNumber);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}