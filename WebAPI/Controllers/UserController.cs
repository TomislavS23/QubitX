using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.DataTransferObjects;
using WebAPI.Models;
using WebAPI.Security;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly QubitXContext _context;
    private readonly IMapper _mapper;

    public UserController(QubitXContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    // READ
    [HttpGet("read/{id:int}")]
    public ActionResult<UserDTO> ReadUser(int id)
    {
        try
        {
            var query = _context.Users.FirstOrDefault(u => u.IdUser == id);

            var result = _mapper.Map<UserDTO>(query);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("read/{username}")]
    public ActionResult<UserDTO> ReadUser(string username)
    {
        try
        {
            var query = _context.Users.FirstOrDefault(u => u.Username == username);

            var result = _mapper.Map<UserDTO>(query);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // UPDATE
    [HttpPut("update"), Authorize]
    public ActionResult<UserDTO> UpdateUser(int id, string firstName = null, string lastName = null, string userName = null)
    {
        try
        {
            var query = _context.Users.FirstOrDefault(u => u.IdUser == id);

            if (firstName != null && query.FirstName != firstName)
            {
                query.FirstName = firstName;
            }
            
            if (lastName != null && query.LastName != lastName)
            {
                query.LastName = lastName;
            }
            
            if (lastName != null && query.Username != userName)
            {
                query.Username = userName;
            }

            var result = _mapper.Map<UserDTO>(query);

            _context.SaveChanges();

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    // DELETE
    [HttpDelete("delete"), Authorize(Roles = "Admin")]
    public ActionResult<UserDTO> DeleteUser(int id)
    {
        try
        {
            var query = _context.Users.FirstOrDefault(u => u.IdUser == id);
            _context.Users.Remove(query);

            var userCourses = _context.UserCourses.Where(uc => uc.IdUser == id);
            _context.RemoveRange(userCourses);

            var courses = _context.Courses.Where(c => c.IdUser == id);
            _context.RemoveRange(courses);

            _context.SaveChanges();
            
            var result = _mapper.Map<UserDTO>(query);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}