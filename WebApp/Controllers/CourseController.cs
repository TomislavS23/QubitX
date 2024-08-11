using System.Net.Http.Headers;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObject;
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

            await _apiService.PostCourseTag(token, courseTags);
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 3,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"User with ID={course.IdUser} created new course with Title={course.CourseTitle}"
            });
            
            return RedirectToAction("Index", "User");
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/Upload caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            return RedirectToAction("Index", "User");
        }
    }
    
    public async Task<IActionResult> View(int courseID)
    {
        var token = HttpContext.Request.Cookies["JWT"];
        
        try
        {
            var course = await _apiService.GetCourseAsync(token, courseID);
            var creator = await _apiService.GetUserAsync(token, course.IdUser.Value);

            var model = new ViewCourseViewModel
            {
                Course = course,
                User = creator
            };

            return View(model);
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/View caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            return RedirectToAction("Index", "User");
        }
    }
    
    public async Task<IActionResult> ViewAndEnroll(int courseID)
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
            
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 5,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"User with ID={initiator.IdUser} has enrolled course with Title={course.CourseTitle}"
            });

            return View(model);
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/ViewAndEnroll caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            return RedirectToAction("Index", "User");
        }
    }
    
    public async Task<IActionResult> Learning()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

        try
        {
            var user = await _apiService.GetUserAsync(token, username);
            var courses = await _apiService.GetCoursesAsync(token);
            var userCourses = await _apiService.GetUserCoursesAsync(token);
            var courseTypes = await _apiService.GetCourseTypesAsync(token);
            var courseTags = await _apiService.GetCourseTags(token);
            var enrolledCourses = new List<CourseDTO>();

            foreach (var uc in userCourses)
            {
                if (user.IdUser == uc.IdUser)
                {
                    var enrolledCourse = courses.FirstOrDefault(c => c.IdCourse == uc.IdCourse);
                    enrolledCourses.Add(enrolledCourse);
                }
            }

            var courseVM = _mapper.Map<IList<CourseViewModel>>(enrolledCourses);
            
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

            return View(courseVM);
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/Learning caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            return RedirectToAction("Index", "User");
        }
    }
    
    public async Task<IActionResult> Manage()
    {
        var token = HttpContext.Request.Cookies["JWT"];
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        
        try
        {
            var user = await _apiService.GetUserAsync(token, username);
            var tags = await _apiService.GetTagsAsync(token);
            var courseTypes = await _apiService.GetCourseTypesAsync(token);
            var courses = await _apiService.GetCoursesCreatedByUser(token, user.IdUser);
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
            
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 4,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"User with ID={user.IdUser} is trying to manage created courses."
            });

            return View(model);
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/Manage caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
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
                LogMessage = $"User with ID={user.IdUser} has deleted course with ID={id}"
            });
            
            return RedirectToAction("Manage");
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/Delete caught an exception:" +
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
        try
        {
            await PrepareData();
            
            var course = await _apiService.GetCourseAsync(token, id);

            var model = _mapper.Map<CourseUploadViewModel>(course);
            
            var courseTags = await _apiService.GetCourseTagsForCourse(token, id);

            ViewBag.CourseTags = courseTags;
            
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 3,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"User with ID={course.IdUser} is trying to edit course with ID={course.IdCourse}"
            });
            
            return View(model);
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/Edit caught an exception:" +
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
                    LogLevel = 4,
                    LogTimestamp = DateTime.Now.ToUniversalTime(),
                    LogMessage = $"User with ID={user.IdUser} has edited course ID={course.IdCourse}"
                });
            
                return RedirectToAction("Manage", "Course");
            }
            catch (Exception e)
            {
                // TODO: Add proper error page
                return RedirectToAction("Index", "User");
            }
        }
        catch (Exception e)
        {
            await _apiService.PostLog(token, new LogDTO
            {
                LogLevel = 1,
                LogTimestamp = DateTime.Now.ToUniversalTime(),
                LogMessage = $"Course/Edit caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
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
                LogMessage = $"Method PrepareData has caught an exception:" +
                             $"Stack Trace: {e.Source}" +
                             $"Stack Trace: {e.StackTrace}" +
                             $"Message: {e.Message}"
            });
            
            Console.WriteLine(e.Message);
        }
    }
}