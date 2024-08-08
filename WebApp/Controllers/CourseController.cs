using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.DataTransferObjects;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class CourseController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public CourseController(IHttpClientFactory client, IMapper mapper)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
        _mapper = mapper;
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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        if (!ModelState.IsValid)
        {
            await PrepareData();
            
            return View(model);
        }
        
        try
        {
            // Find real user ID
            var username = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name).Value;
            var userResponse = await _httpClient.GetAsync($"api/user/read/{username}");
            userResponse.EnsureSuccessStatusCode();

            // Create user object and assign user id to course
            var user = await userResponse.Content.ReadFromJsonAsync<UserDTO>();
            var course = _mapper.Map<CourseDTO>(model);
            course.IdUser = user.IdUser;
            
            // Convert course to json and upload it
            var json = JsonConvert.SerializeObject(course);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/course/create", content);
            if (!response.IsSuccessStatusCode)
            {
                return View(model);
            }

            var courseId = await response.Content.ReadFromJsonAsync<int>();
            var courseTags = model.Tags.Select(tag => new CourseTagDTO { IdCourse = courseId, IdTag = tag }).ToList();
            
            var courseTagJson = JsonConvert.SerializeObject(courseTags);
            var courseTagContent = new StringContent(courseTagJson, Encoding.UTF8, "application/json");
            var insertCourseTag = await _httpClient.PostAsync("api/course-tag/insert-multiple", courseTagContent);

            if (!insertCourseTag.IsSuccessStatusCode)
            {
                return View(model);
            }

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
        var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        try
        {
            var responseCourse = await _httpClient.GetAsync($"api/course/read?id={courseID}");
            responseCourse.EnsureSuccessStatusCode();

            var course = await responseCourse.Content.ReadFromJsonAsync<CourseDTO>();
            
            var responseCreator = await _httpClient.GetAsync($"api/user/read/{course.IdUser}");
            responseCreator.EnsureSuccessStatusCode();

            var creator = await responseCreator.Content.ReadFromJsonAsync<UserDTO>();

            var model = new ViewCourseViewModel
            {
                Course = course,
                User = creator
            };
            
            // TODO: Register user to course

            return View(model);
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "User");
        }
    }

    private async Task PrepareData()
    {
        var courseResponse = await _httpClient.GetAsync("api/coursetypes/read");
        courseResponse.EnsureSuccessStatusCode();

        var tagResponse = await _httpClient.GetAsync("api/tag/readtags");
        tagResponse.EnsureSuccessStatusCode();

        var courseTypes = await courseResponse.Content.ReadFromJsonAsync<IEnumerable<CourseTypeViewModel>>();
        var tags = await tagResponse.Content.ReadFromJsonAsync<IEnumerable<TagViewModel>>();

        ViewBag.CourseTypes = courseTypes;
        ViewBag.Tags = tags;
    }
}