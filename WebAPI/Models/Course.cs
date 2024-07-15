using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Course
{
    public int IdCourse { get; set; }

    public int? IdUser { get; set; }

    public int? IdCourseType { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<CourseSection> CourseSections { get; } = new List<CourseSection>();

    public virtual ICollection<CourseTag> CourseTags { get; } = new List<CourseTag>();

    public virtual CourseType? IdCourseTypeNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<UserCourse> UserCourses { get; } = new List<UserCourse>();
}
