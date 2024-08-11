using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObject;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class TagController : Controller
{
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public TagController(IMapper mapper, IApiService service)
    {
        _mapper = mapper;
        _apiService = service;
    }
    
    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        var tags = await _apiService.GetTagsAsync(token);
        var model = _mapper.Map<IList<TagViewModel>>(tags);

        return View(model);
    }
    
    public async Task<IActionResult> Add(string title)
    {
        try
        {
            var token = HttpContext.Request.Cookies["JWT"];

            await _apiService.PostTag(token, new TagDTO
            {
                TagTitle = title
            });

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return RedirectToAction("Manage", "Admin");
        }
    }
    
    public async Task<IActionResult> Edit(int id, string title)
    {
        try
        {
            var token = HttpContext.Request.Cookies["JWT"];

            await _apiService.PutTag(token, new TagDTO
            {
                IdTag = id,
                TagTitle = title
            });

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return RedirectToAction("Manage", "Admin");
        }
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var token = HttpContext.Request.Cookies["JWT"];

            await _apiService.DeleteTag(token, id);

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return RedirectToAction("Manage", "Admin");
        }
    }
}