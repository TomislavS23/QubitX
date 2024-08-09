using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.DataTransferObjects;

namespace WebApp.Services;

public interface IApiService
{
    // ACCOUNT
    Task<string> LoginAsync(string username, string password);
    Task<string> RegisterAsync(RegisterDTO data);
    
    // COURSE
    Task<CourseDTO> GetCourseAsync(string token, int courseID);
    Task<IList<CourseDTO>> GetCoursesAsync(string token);
    
    // CourseTypes
    Task<IList<CourseTypeDTO>> GetCourseTypesAsync(string token);
    
    // TAG
    Task<IList<TagDTO>> GetTagsAsync(string token);
    
    // USER
    Task<UserDTO> GetUserAsync(string token, string username);
    Task<UserDTO> GetUserAsync(string token, int idUser);
    
    // USER-COURSE
    Task PostUserCourseAsync(string token, UserCourseDTO userCourse);
    Task<int> PostCourse(string token, CourseDTO course);
    Task PostCourseTag(string token, IList<CourseTagDTO> courseTags);
    
    // COURSE-TAG
    Task<IList<CourseTagDTO>> GetCourseTags(string token);
}