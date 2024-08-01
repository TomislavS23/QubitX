using System.Xml.XPath;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/tag/")]
public class TagController : Controller
{
    private readonly QubitXContext _context;
    private readonly IMapper _mapper;

    public TagController(QubitXContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpPost("create"), Authorize(Roles = "Admin")]
    public ActionResult<TagDTO> CreateTag(string tagTitle)
    {
        try
        {
            var tag = new Tag
            {
                TagTitle = tagTitle
            };
            
            var query = _context.Tags.Add(tag);
            Console.WriteLine(tag.IdTag);

            _context.SaveChanges();
            Console.WriteLine(tag.IdTag);

            var result = _mapper.Map<TagDTO>(tag);

            // fun fact: I wasted 30min+ of my valuable life debugging why this =>
            // return Ok(query); always returns exception when inserting new tag
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("read"), Authorize]
    public ActionResult<TagDTO> ReadTag(int id)
    {
        try
        {
            var query = _context.Tags.FirstOrDefault(t => t.IdTag == id);

            var result = _mapper.Map<TagDTO>(query);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("readtags")]
    public ActionResult<TagDTO> ReadTags()
    {
        try
        {
            var query = _context.Tags.OrderBy(t => t.IdTag).ToArray();

            var result = _mapper.Map<IEnumerable<TagDTO>>(query);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("update"), Authorize(Roles = "Admin")]
    public ActionResult<TagDTO> UpdateTag(int id, string newTitle = null)
    {
        try
        {
            var query = _context.Tags.FirstOrDefault(t => t.IdTag == id);

            if (query != null && newTitle != null)
            {
                query.TagTitle = newTitle;
            }
            
            _context.SaveChanges();

            var result = _mapper.Map<TagDTO>(query);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("delete"), Authorize(Roles = "Admin")]
    public ActionResult DeleteTag(int id)
    {
        try
        {
            var query = _context.Tags.FirstOrDefault(t => t.IdTag == id);
            _context.Tags.Remove(query);

            var courseTags = _context.CourseTags.Where(ct => ct.IdTag == id);
            _context.CourseTags.RemoveRange(courseTags);
            
            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}