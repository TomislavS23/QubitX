using System.Collections;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using WebApp.DataTransferObjects;

namespace WebApp.Services;

public class ApiService : IApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public ApiService(IHttpClientFactory client)
    {
        _httpClientFactory = client;
        _httpClient = _httpClientFactory.CreateClient("httpclient"); 
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var response = await _httpClient.GetAsync($"api/auth/login?username={username}&password={password}");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> RegisterAsync(RegisterDTO data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/auth/register", content);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<CourseDTO> GetCourseAsync(string token, int courseID)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"api/course/read?id={courseID}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CourseDTO>();
    }

    public async Task<IList<CourseDTO>> GetCoursesAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync("api/course/read-courses");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IList<CourseDTO>>();
    }

    public async Task<IList<CourseDTO>> GetCoursesCreatedByUser(string token, int idUser)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync("api/course/read-courses");
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<IList<CourseDTO>>();

        return result.Where(r => r.IdUser == idUser).ToList();
    }

    public async Task EditCourse(string token, CourseDTO data)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        await _httpClient.PutAsync("api/course/update", content);
    }

    public async Task DeleteCourse(string token, int id)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"api/course/delete?id={id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<IList<CourseTypeDTO>> GetCourseTypesAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync("api/coursetypes/read");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<CourseTypeDTO>>();
    }

    public async Task<IList<TagDTO>> GetTagsAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync("api/tag/readtags");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<IList<TagDTO>>();
    }

    public async Task<UserDTO> GetUserAsync(string token, string username)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"api/user/read/{username}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDTO>();
    }

    public async Task<UserDTO> GetUserAsync(string token, int userId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync($"api/user/read/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDTO>();
    }

    public async Task PostUserCourseAsync(string token, UserCourseDTO userCourse)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var payload = JsonConvert.SerializeObject(userCourse);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/user-course/add", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> PostCourse(string token, CourseDTO course)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var payload = JsonConvert.SerializeObject(course);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/course/create", content);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task PostCourseTag(string token, IList<CourseTagDTO> courseTags)
    {
        var payload = JsonConvert.SerializeObject(courseTags);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/course-tag/insert-multiple", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IList<UserCourseDTO>> GetUserCoursesAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("api/user-course/read");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IList<UserCourseDTO>>();
    }

    public async Task<IList<CourseTagDTO>> GetCourseTags(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync("api/course-tag/read");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IList<CourseTagDTO>>();
    }

    public async Task<IList<CourseTagDTO>> GetCourseTagsForCourse(string token, int idCourse)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.GetAsync("api/course-tag/read");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<IList<CourseTagDTO>>();

        return result.Where(ct => ct.IdCourse == idCourse).ToList();
    }

    public async Task DeleteCourseTags(string token, int idCourse)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"api/course-tag/delete/{idCourse}");
        response.EnsureSuccessStatusCode();
    }
}