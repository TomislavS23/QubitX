using System.Net.Http.Headers;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize]
public class CourseController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public CourseController(IHttpClientFactory client, IMapper mapper, IApiService service)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
        _mapper = mapper;
        _apiService = service;
    }
    
    public async Task<IActionResult> Upload()
    {
        try
        {
            await PrepareData();

            return View();
        }
        catch (Exception e)
        {
            return View(); // TODO: Make new error MVC
        }
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(CourseUploadViewModel model)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name).Value;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        if (!ModelState.IsValid)
        {
            await PrepareData();
            
            return View(model);
        }
        
        try
        {
            var user = await _apiService.GetUserAsync(token, username);
            var course = _mapper.Map<CourseDTO>(model);
            course.IdUser = user.IdUser;

            var courseId = await _apiService.PostCourse(token, course);
            var courseTags = model.Tags.Select(tag => new CourseTagDTO { IdCourse = courseId, IdTag = tag }).ToList();

            _apiService.PostCourseTag(token, courseTags);
            
            return RedirectToAction("Index", "User");
        }
        catch (Exception e)
        {
            // TODO: Add proper error page
            return RedirectToAction("Index", "User");
        }
    }
    
    public async Task<IActionResult> View(int courseID)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        try
        {
            var course = await _apiService.GetCourseAsync(token, courseID);
            var creator = await _apiService.GetUserAsync(token, course.IdUser.Value);
            var initiator = await _apiService.GetUserAsync(token, username);

            var model = new ViewCourseViewModel
            {
                Course = course,
                User = creator
            };

            var userCourse = new UserCourseDTO
            {
                IdCourse = course.IdCourse,
                IdUser = initiator.IdUser
            };

            await _apiService.PostUserCourseAsync(token, userCourse);

            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "User");
        }
    }
    
    public IActionResult Learning()
    {
        // TODO: Prepare all enrolled courses for showcase (grid, just like in main).
        return View();
    }

    private async Task PrepareData()
    {
        try
        {
            var token = HttpContext.Request.Cookies["JWT"];

            ViewBag.CourseTypes = _mapper.Map<IEnumerable<CourseTypeViewModel>>(await _apiService.GetCourseTypesAsync(token));
            ViewBag.Tags = _mapper.Map<IEnumerable<TagViewModel>>(await _apiService.GetTagsAsync(token));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}