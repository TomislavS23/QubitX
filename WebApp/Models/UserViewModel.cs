namespace WebApp.Models;

public class UserViewModel
{
    public IEnumerable<TagViewModel> Tags { get; set; }
    public IEnumerable<CourseTypeViewModel> CourseTypes { get; set; }
}