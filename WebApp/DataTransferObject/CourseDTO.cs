namespace WebApp.DataTransferObjects;

public class CourseDTO
{
    public int IdCourse { get; set; }

    public int? IdUser { get; set; }

    public int? IdCourseType { get; set; }

    public string? CourseTitle { get; set; }

    public string? CourseContent { get; set; }
}