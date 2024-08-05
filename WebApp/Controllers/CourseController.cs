using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class CourseController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public CourseController(IHttpClientFactory client)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
    }
    
    public async Task<IActionResult> Upload()
    {
        try
        {
            var courseResponse = await _httpClient.GetAsync("api/coursetypes/read");
            courseResponse.EnsureSuccessStatusCode();

            var tagResponse = await _httpClient.GetAsync("api/tag/readtags");
            tagResponse.EnsureSuccessStatusCode();

            var courseTypes = await courseResponse.Content.ReadFromJsonAsync<IEnumerable<CourseTypeViewModel>>();
            var tags = await tagResponse.Content.ReadFromJsonAsync<IEnumerable<TagViewModel>>();

            ViewBag.CourseTypes = courseTypes;
            ViewBag.Tags = tags;

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
            var courseResponse = await _httpClient.GetAsync("api/coursetypes/read");
            courseResponse.EnsureSuccessStatusCode();

            var tagResponse = await _httpClient.GetAsync("api/tag/readtags");
            tagResponse.EnsureSuccessStatusCode();

            var courseTypes = await courseResponse.Content.ReadFromJsonAsync<IEnumerable<CourseTypeViewModel>>();
            var tags = await tagResponse.Content.ReadFromJsonAsync<IEnumerable<TagViewModel>>();

            ViewBag.CourseTypes = courseTypes;
            ViewBag.Tags = tags;
            
            return View(model);
        }
        
        try
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/course/create", content);
            
            if (!response.IsSuccessStatusCode)
            {
                return View();
            }
            
            return RedirectToAction("Index", "User");
        }
        catch (Exception e)
        {
            // TODO: Add proper error page
            return RedirectToAction("Index", "User");
        }
    }
}