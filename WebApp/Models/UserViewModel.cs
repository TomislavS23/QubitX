namespace WebApp.Models;

public class UserViewModel
{
    public IEnumerable<TagViewModel> Tags { get; set; }
    public IEnumerable<CourseTypeViewModel> CourseTypes { get; set; }
    public IEnumerable<CourseViewModel> Courses { get; set; }
}