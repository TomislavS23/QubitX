using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/course/")]
public class CourseController : Controller
{
    private readonly QubitXContext _context;
    private readonly IMapper _mapper;

    public CourseController(QubitXContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    // CREATE
    [HttpPost("create"), Authorize]
    public ActionResult CreateCourse([FromBody] CreateCourseDTO data)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == data.Username);
            
            var course = new CourseDTO()
            {
                IdUser = user.IdUser,
                IdCourseType = data.IdCourseType,
                CourseTitle = data.CourseTitle,
                CourseContent = data.CourseContent
            };

            var result = _mapper.Map<Course>(course);

            _context.Courses.Add(result);
            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // READ

    [HttpGet("read"), Authorize]
    public ActionResult<CourseDTO> ReadCourse(int id)
    {
        try
        {
            var query = _context.Courses.FirstOrDefault(c => c.IdCourse == id);

            var result = _mapper.Map<CourseDTO>(query);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("readcourses"), Authorize]
    public ActionResult<IEnumerable<CourseDTO>> ReadCourses()
    {
        try
        {
            var query = _context.Courses.ToList();
            
            var result = _mapper.Map<IEnumerable<CourseDTO>>(query);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // UPDATE
    [HttpPut("update"), Authorize]
    public ActionResult<CourseDTO> UpdateCourse(int id, int courseType, string courseTitle = null,
        string courseContent = null)
    {
        try
        {
            var query = _context.Courses.FirstOrDefault(c => c.IdCourse == id);

            query.IdCourseType = courseType;

            if (query != null && courseTitle != null)
            {
                query.CourseTitle = courseTitle;
            }

            if (query != null && courseContent != null)
            {
                query.CourseContent = courseContent;
            }

            _context.SaveChanges();
            
            var result = _mapper.Map<CourseDTO>(query);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // DELETE
    [HttpDelete("delete"), Authorize]
    public ActionResult DeleteCourse(int id)
    {
        try
        {
            var query = _context.Courses.FirstOrDefault(c => c.IdCourse == id);
            _context.Courses.Remove(query);

            var userCourses = _context.UserCourses.Where(uc => uc.IdCourse == id);
            _context.UserCourses.RemoveRange(userCourses);

            var courseTag = _context.CourseTags.Where(ct => ct.IdCourse == id);
            _context.CourseTags.RemoveRange(courseTag);

            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}