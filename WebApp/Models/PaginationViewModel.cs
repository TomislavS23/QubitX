namespace WebApp.Models;

public class PaginationViewModel
{
    public IList<CourseViewModel> Courses { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int ContentPerPage { get; set; }
    public int CourseTypeId { get; set; }
}