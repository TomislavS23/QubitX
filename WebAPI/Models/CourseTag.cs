using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class CourseTag
{
    public int IdCourseTag { get; set; }

    public int? IdCourse { get; set; }

    public int? IdTag { get; set; }

    public virtual Course? IdCourseNavigation { get; set; }

    public virtual Tag? IdTagNavigation { get; set; }
}
