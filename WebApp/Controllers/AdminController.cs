using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObject;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IApiService _apiService;
    private readonly IMapper _mapper;

    public AdminController(IMapper mapper, IApiService service)
    {
        _mapper = mapper;
        _apiService = service;
    }

    public async Task<IActionResult> Search(string title)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        try
        {
            var tags = await _apiService.GetTagsAsync(token);
            var courseTypes = await _apiService.GetCourseTypesAsync(token);
            var courses = await _apiService.GetCoursesAsync(token, title);
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
            return RedirectToAction("Index", "User");
        }
    }
    
    
    
    public async Task<IActionResult> Manage()
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
            return RedirectToAction("Index", "User");
        }
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        try
        {
            await _apiService.DeleteCourse(token, id);
            var user = await _apiService.GetUserAsync(token, username);
            
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Administrator (ID={user.IdUser} has deleted course ID={id}"
            });
            
            return RedirectToAction("Manage");
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Admin/Delete caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            return RedirectToAction("Index", "User");
        }
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        try
        {
            await PrepareData();
            
            var course = await _apiService.GetCourseAsync(token, id);
            var user = await _apiService.GetUserAsync(token, username);

            var model = _mapper.Map<CourseUploadViewModel>(course);
            
            var courseTags = await _apiService.GetCourseTagsForCourse(token, id);

            ViewBag.CourseTags = courseTags;
            
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 3,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Administrator (ID={user.IdUser} is trying to edit course ID={id}"
            });
            
            return View(model);
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Admin/Edit caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            return RedirectToAction("Index", "User");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(CourseUploadViewModel model)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        try
        {
            if (!ModelState.IsValid)
            {
                await PrepareData();
            
                return View(model);
            }

            try
            {
                var course = _mapper.Map<CourseDTO>(model);
                var user = await _apiService.GetUserAsync(token, username);

                await _apiService.EditCourse(token, course);
                
                var courseTags = model.Tags.Select(tag => new CourseTagDTO { IdCourse = model.IdCourse, IdTag = tag }).ToList();
                await _apiService.DeleteCourseTags(token, course.IdCourse);
                await _apiService.PostCourseTag(token, courseTags);
                
                await _apiService.PostLog(token, new LogDTO
                {
                    LogLevel = 3,
                    LogTimestamp = DateTime.Now.ToUniversalTime(),
                    LogMessage = $"Administrator (ID={user.IdUser} has edited course with ID={model.IdCourse}"
                });
            
                return RedirectToAction("Manage", "Admin");
            }
            catch (Exception e)
            {
                await _apiService.PostLog(token, new LogDTO
                {
                    LogLevel = 1,
                    LogTimestamp = DateTime.Now.ToUniversalTime(),
                    LogMessage = $"Admin/Edit caught an exception:" +
                                 $"Stack Trace: {e.Source}" +
                                 $"Stack Trace: {e.StackTrace}" +
                                 $"Message: {e.Message}"
                });
                
                return RedirectToAction("Index", "User");
            }
        }
        catch (Exception e)
        {
            return RedirectToAction("Index", "User");
        }
    }
    
    private async Task PrepareData()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        try
        {
            ViewBag.CourseTypes = _mapper.Map<IEnumerable<CourseTypeViewModel>>(await _apiService.GetCourseTypesAsync(token));
            ViewBag.Tags = _mapper.Map<IEnumerable<TagViewModel>>(await _apiService.GetTagsAsync(token));
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Method PrepareData() has caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            Console.WriteLine(e.Message);
        }
    }
}