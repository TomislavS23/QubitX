namespace WebApp.Models;

public class UserViewModel
{
    public IList<TagViewModel> Tags { get; set; }
    public IList<CourseTypeViewModel> CourseTypes { get; set; }
    public IList<CourseViewModel> Courses { get; set; }
}