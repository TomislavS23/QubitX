using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/logs")]
public class LogController : Controller
{
    private readonly QubitXContext _context;
    private readonly IMapper _mapper;

    public LogController(QubitXContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpGet("get/{n}"), Authorize(Roles = "Admin")]
    [Authorize(Roles = "Admin")]
    public ActionResult<IEnumerable<Log>> Get(int n = 10)
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

    [HttpGet("count"), Authorize(Roles = "Admin")]
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

    [HttpPost("create"), Authorize(Roles = "Admin")]
    public ActionResult CreateLog([FromBody] LogDTO data)
    {
        try
        {
            var log = _mapper.Map<Log>(data);

            _context.Logs.Add(log);
            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}