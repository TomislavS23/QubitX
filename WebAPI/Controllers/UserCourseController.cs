using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/user-course/")]
public class UserCourseController : Controller
{
    private readonly QubitXContext _context;
    private readonly IMapper _mapper;

    public UserCourseController(QubitXContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpPost("add"), Authorize]
    public ActionResult Add([FromBody] UserCourseDTO data)
    {
        var obj = _mapper.Map<UserCourse>(data);
        _context.UserCourses.Add(obj);

        _context.SaveChanges();

        return Ok();
    }
    
    [HttpGet("read"), Authorize]
    public ActionResult ReadMany()
    {
        var query = _context.UserCourses.ToList();

        var result = _mapper.Map<IEnumerable<UserCourseDTO>>(query);

        return Ok(result);
    }
}