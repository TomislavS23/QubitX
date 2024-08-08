using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class CourseUploadViewModel
{
    public int IdCourseType { get; set; }
    [Required(ErrorMessage = "Please enter the title of your course!")]
    public string CourseTitle { get; set; }
    
    [Required(ErrorMessage = "Your course should have content!")]
    public string CourseContent { get; set; }

    public IList<int> Tags { get; set; }
}