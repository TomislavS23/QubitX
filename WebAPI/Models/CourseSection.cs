using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class CourseSection
{
    public int IdCourseSection { get; set; }

    public int? IdCourse { get; set; }

    public int? IdSectionType { get; set; }

    public string? Content { get; set; }

    public virtual Course? IdCourseNavigation { get; set; }

    public virtual SectionType? IdSectionTypeNavigation { get; set; }
}
