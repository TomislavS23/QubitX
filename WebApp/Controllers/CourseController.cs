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
            var response = await _httpClient.GetAsync("api/coursetypes/read");
            response.EnsureSuccessStatusCode();

            var courseTypes = await response.Content.ReadFromJsonAsync<IEnumerable<CourseTypeViewModel>>();

            ViewBag.CourseTypes = courseTypes;

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
        try
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/course/create", content);
            
            if (!response.IsSuccessStatusCode)
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
}