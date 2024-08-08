using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataTransferObjects;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public UserController(IHttpClientFactory client, IMapper mapper)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
        _mapper = mapper;
    }
    
    [Route("/main")]
    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        try
        {
            var responseTags = await _httpClient.GetAsync("api/tag/readtags");
            responseTags.EnsureSuccessStatusCode();
            
            var responseCourseTypes = await _httpClient.GetAsync("api/coursetypes/read");
            responseCourseTypes.EnsureSuccessStatusCode();

            var responseCourses = await _httpClient.GetAsync("api/course/read-courses");
            responseCourses.EnsureSuccessStatusCode();

            var responseCourseTag = await _httpClient.GetAsync("api/course-tag/read");
            responseCourseTag.EnsureSuccessStatusCode();

            var tags = await responseTags.Content.ReadFromJsonAsync<IEnumerable<TagViewModel>>();
            var courseTypes = await responseCourseTypes.Content.ReadFromJsonAsync<IEnumerable<CourseTypeViewModel>>();
            var courses = await responseCourses.Content.ReadFromJsonAsync<IEnumerable<CourseDTO>>();
            var courseTags = await responseCourseTag.Content.ReadFromJsonAsync<IEnumerable<CourseTagDTO>>();

            var courseVM = _mapper.Map<IEnumerable<CourseViewModel>>(courses);

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
                Tags = tags,
                CourseTypes = courseTypes,
                Courses = courseVM
            };

            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}