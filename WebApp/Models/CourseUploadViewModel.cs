using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class CourseUploadViewModel
{
    [Required(ErrorMessage = "Your course should have course type!")]
    public int CourseType { get; set; }
    [Required(ErrorMessage = "Please enter the title of your course!")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Your course should have content!")]
    public string Content { get; set; }    
}