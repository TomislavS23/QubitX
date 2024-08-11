using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObject;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class CourseTypesController : Controller
{
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public CourseTypesController(IMapper mapper, IApiService service)
    {
        _mapper = mapper;
        _apiService = service;
    }
    
    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        var courseTypes = await _apiService.GetCourseTypesAsync(token);
        var model = _mapper.Map<IList<CourseTypeViewModel>>(courseTypes);

        return View(model);
    }
    
    public async Task<IActionResult> Add(string title)
    {
        try
        {
            var token = HttpContext.Request.Cookies["JWT"];

            await _apiService.PostCourseType(token, new CourseTypeDTO
            {
                CourseTypeTitle = title
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

            await _apiService.PutCourseType(token, new CourseTypeDTO
            {
                IdCourseType = id,
                CourseTypeTitle = title
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

            await _apiService.DeleteCourseType(token, id);

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            return RedirectToAction("Manage", "Admin");
        }
    }
}