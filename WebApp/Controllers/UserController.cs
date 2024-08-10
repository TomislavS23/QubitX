using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObjects;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public UserController(IHttpClientFactory client, IMapper mapper, IApiService apiService)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
        _mapper = mapper;
        _apiService = apiService;
    }
    
    [Route("/main")]
    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        try
        {
            var tags = await _apiService.GetTagsAsync(token);
            var courseTypes = await _apiService.GetCourseTypesAsync(token);
            var courses = await _apiService.GetCoursesAsync(token);
            var courseTags = await _apiService.GetCourseTags(token);

            var courseVM = _mapper.Map<IList<CourseViewModel>>(courses);

            foreach (var course in courseVM)
            {
                course.CourseTypeTitle = courseTypes.FirstOrDefault(ct => ct.IdCourseType == course.IdCourseType).CourseTypeTitle;
                
                if (course.Tags == null)
                {
                    course.Tags = new List<CourseTagDTO>();
                }
                
                foreach (var ct in courseTags.Where(ct => ct.IdCourse == course.IdCourse))
                {
                    course.Tags.Add(ct);
                }
            }

            var model = new UserViewModel
            {
                Tags = _mapper.Map<IList<TagViewModel>>(tags),
                CourseTypes = _mapper.Map<IList<CourseTypeViewModel>>(courseTypes),
                Courses = courseVM
            };

            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "Home");
        }
    }
    
    public async Task<IActionResult> Search(string title)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        try
        {
            var courses = await _apiService.GetCoursesAsync(token, title);
            var courseTypes = await _apiService.GetCourseTypesAsync(token);
            var courseTags = await _apiService.GetCourseTags(token);

            var model = _mapper.Map<IList<CourseViewModel>>(courses);

            foreach (var course in model)
            {
                course.CourseTypeTitle = courseTypes.FirstOrDefault(ct => ct.IdCourseType == course.IdCourseType).CourseTypeTitle;
                
                if (course.Tags == null)
                {
                    course.Tags = new List<CourseTagDTO>();
                }
                
                foreach (var ct in courseTags.Where(ct => ct.IdCourse == course.IdCourse))
                {
                    course.Tags.Add(ct);
                }
            }

            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "User");
        }
    }
}