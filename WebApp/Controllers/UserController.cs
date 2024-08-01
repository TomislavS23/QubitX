using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public UserController(IHttpClientFactory client)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient"); 
        
        // var token = HttpContext.Request.Cookies["JWT"];
        // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    
    // GET => /main
    [Route("/main")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var responseTags = await _httpClient.GetAsync("api/tag/readtags");
            responseTags.EnsureSuccessStatusCode();
            
            var responseCourseTypes = await _httpClient.GetAsync("api/coursetypes/read");
            responseCourseTypes.EnsureSuccessStatusCode();

            // TODO: make proper viewmodel for courses
            
            /*
            var responseCourses = await _httpClient.GetAsync("api/course/readcourses");
            responseCourses.EnsureSuccessStatusCode();
            */

            var tags = await responseTags.Content.ReadFromJsonAsync<IEnumerable<TagViewModel>>();
            var courseTypes = await responseCourseTypes.Content.ReadFromJsonAsync<IEnumerable<CourseTypeViewModel>>();

            var model = new UserViewModel
            {
                Tags = tags,
                CourseTypes = courseTypes
            };

        return View(model);
        }
        catch (Exception e)
        {
            return View(); // TODO: Make new error MVC
        }
    }
}