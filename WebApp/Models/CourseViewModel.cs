using WebApp.DataTransferObject;

namespace WebApp.Models;

public class CourseViewModel
{
    public int IdCourse { get; set; }

    public string? CourseTitle { get; set; }
    
    public int? IdCourseType { get; set; }
    
    public string? CourseTypeTitle { get; set; }

    public IList<CourseTagDTO>? Tags { get; set; }
}