using System.Collections;
using WebApp.DataTransferObjects;

namespace WebApp.Services;

public interface IApiService
{
    // ACCOUNT
    Task<string> LoginAsync(string username, string password);
    Task<string> RegisterAsync(RegisterDTO data);
    
    // COURSE
    Task<CourseDTO> GetCourseAsync(string token, int courseID);
    Task<IList<CourseDTO>> GetCoursesAsync(string token, string title);
    Task<IList<CourseDTO>> GetCoursesAsync(string token);
    Task<IList<CourseDTO>> GetCoursesCreatedByUser(string token, int idUser);
    Task EditCourse(string token, CourseDTO data);
    Task DeleteCourse(string token, int id);
    
    // CourseTypes
    Task<IList<CourseTypeDTO>> GetCourseTypesAsync(string token);
    
    // TAG
    Task<IList<TagDTO>> GetTagsAsync(string token);
    
    // USER
    Task<UserDTO> GetUserAsync(string token, string username);
    Task<UserDTO> GetUserAsync(string token, int idUser);
    Task PutUserAsync(string token, UserDTO data);
    
    // USER-COURSE
    Task PostUserCourseAsync(string token, UserCourseDTO userCourse);
    Task<int> PostCourse(string token, CourseDTO course);
    Task PostCourseTag(string token, IList<CourseTagDTO> courseTags);
    Task<IList<UserCourseDTO>> GetUserCoursesAsync(string token);
    
    // COURSE-TAG
    Task<IList<CourseTagDTO>> GetCourseTags(string token);
    Task<IList<CourseTagDTO>> GetCourseTagsForCourse(string token, int idCourse);
    Task DeleteCourseTags(string token, int idCourse);
}