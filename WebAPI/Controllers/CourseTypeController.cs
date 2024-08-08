using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/coursetypes/")]
public class CourseTypeController : Controller
{
    private readonly QubitXContext _context;
    private readonly IMapper _mapper;

    public CourseTypeController(QubitXContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // CREATE
    [HttpPost("create")]
    public ActionResult CreateCourseType(string title)
    {
        try
        {
            var query = _context.CourseTypes.Add(new CourseType
            {
                CourseTypeTitle = title
            });

            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    // READ
    [HttpGet("read")]
    public ActionResult<IEnumerable<CourseTypeDTO>> ReadCourseTypes()
    {
        try
        {
            var query = _context.CourseTypes.OrderBy(ct => ct.IdCourseType).ToArray();

            var result = _mapper.Map<IEnumerable<CourseTypeDTO>>(query);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    // UPDATE
    [HttpPut("update")]
    public ActionResult<CourseTypeDTO> UpdateCourseType(int id, string newTitle = null)
    {
        try
        {
            var obj = _context.CourseTypes.FirstOrDefault(ct => ct.IdCourseType == id);

            if (obj != null && newTitle != null)
            {
                obj.CourseTypeTitle = newTitle;
            }

            _context.SaveChanges();

            var result = _mapper.Map<CourseTypeDTO>(obj);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    // DELETE
    [HttpDelete("delete")]
    public ActionResult DeleteCourseType(int id)
    {
        try
        {
            var query = _context.CourseTypes.FirstOrDefault(ct => ct.IdCourseType == id);
            _context.CourseTypes.Remove(query);

            var courses = _context.Courses.Where(c => c.IdCourseType == id);
            _context.Courses.RemoveRange(courses);

            _context.SaveChanges();
            
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}