using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Controller]
[Route("api/course-tag/")]
public class CourseTagController : Controller
{
    private readonly QubitXContext _context;
    private readonly IMapper _mapper;

    public CourseTagController(QubitXContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpPost("insert"), Authorize]
    public ActionResult InsertCourseTag([FromBody] CourseTagDTO courseTag)
    {
        try
        {
            var ct = _mapper.Map<CourseTag>(courseTag);
            _context.CourseTags.Add(ct);

            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.InnerException);
        }
    }
    
    [HttpPost("insert-multiple"), Authorize]
    public ActionResult InsertCourseTag([FromBody] IList<CourseTagDTO> courseTags)
    {
        try
        {
            foreach (var courseTag in courseTags)
            {
                var ct = _mapper.Map<CourseTag>(courseTag);
                _context.CourseTags.Add(ct);
            }

            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("read"), Authorize]
    public ActionResult ReadCourseTag()
    {
        try
        {
            var query = _context.CourseTags.ToList();

            var result = _mapper.Map<IEnumerable<CourseTagDTO>>(query);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}