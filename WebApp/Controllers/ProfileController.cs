using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApp.DataTransferObjects;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public ProfileController(IHttpClientFactory client, IMapper mapper, IApiService service)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient");
        _mapper = mapper;
        _apiService = service;
    }
    
    public async Task<IActionResult> ProfileDetails()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = HttpContext.User.Identity.Name;

        var user = await _apiService.GetUserAsync(token, username);
        var model = _mapper.Map<ProfileViewModel>(user);

        return View(model);
    }
    
    public async Task<IActionResult> ProfileEdit(string username)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var user = await _apiService.GetUserAsync(token, username);
        var model = _mapper.Map<ProfileViewModel>(user);

        return View(model);
    }
    
    [HttpPut]
    public async Task<IActionResult> ProfileEdit(ProfileViewModel model)
    {
        var token = HttpContext.Request.Cookies["JWT"];

        var user = _mapper.Map<UserDTO>(model);

        await _apiService.PutUserAsync(token, user);

        return RedirectToAction("ProfileDetails");
    }
    
    public async Task<JsonResult> GetProfileData(int id)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var user = await _apiService.GetUserAsync(token, id);
        return Json(new {
            user.FirstName,
            user.LastName,
            user.Username,
        });
    }

    [HttpPut]
    public async Task<IActionResult> SaveProfileData(int id, [FromBody] ProfileViewModel data)
    {
        data.IdUser = id;
        
        var token = HttpContext.Request.Cookies["JWT"];
        var updatedData = _mapper.Map<UserDTO>(data);

        await _apiService.PutUserAsync(token, updatedData);

        return Ok();
    }
}