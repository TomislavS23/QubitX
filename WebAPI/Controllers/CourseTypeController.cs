using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [HttpPost("create"), Authorize]
    public ActionResult CreateCourseType([FromBody] CourseTypeDTO data)
    {
        try
        {
            var query = _context.CourseTypes.Add(new CourseType
            {
                CourseTypeTitle = data.CourseTypeTitle
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
    [HttpGet("read"), Authorize]
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
    [HttpPut("update"), Authorize]
    public ActionResult<CourseTypeDTO> UpdateCourseType([FromBody] CourseTypeDTO data)
    {
        try
        {
            var obj = _context.CourseTypes.FirstOrDefault(ct => ct.IdCourseType == data.IdCourseType);

            obj.CourseTypeTitle = data.CourseTypeTitle;

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
            var courses = _context.Courses.Where(c => c.IdCourseType == id).ToList();

            foreach (var course in courses)
            {
                var courseTags = _context.CourseTags.Where(ct => ct.IdCourse == course.IdCourse);
                _context.RemoveRange(courseTags);

                var userCourse = _context.UserCourses.Where(uc => uc.IdCourse == course.IdCourse);
                _context.RemoveRange(userCourse);
            }
            
            _context.Courses.RemoveRange(courses);
            
            var query = _context.CourseTypes.FirstOrDefault(ct => ct.IdCourseType == id);
            _context.CourseTypes.Remove(query);

            _context.SaveChanges();
            
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}