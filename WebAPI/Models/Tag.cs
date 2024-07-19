using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Tag
{
    public int IdTag { get; set; }

    public string? TagTitle { get; set; }

    public virtual ICollection<CourseTag> CourseTags { get; } = new List<CourseTag>();
}
