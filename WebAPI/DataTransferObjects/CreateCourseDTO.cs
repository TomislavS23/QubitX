namespace WebAPI.DataTransferObjects;

public class CreateCourseDTO
{
    public string? Username { get; set; }

    public int? IdCourseType { get; set; }

    public string? CourseTitle { get; set; }

    public string? CourseContent { get; set; }
    public IEnumerable<int> Tags { get; set; }
}