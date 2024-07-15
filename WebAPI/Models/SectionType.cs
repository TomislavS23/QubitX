using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class SectionType
{
    public int IdSectionType { get; set; }

    public string? SectionType1 { get; set; }

    public virtual ICollection<CourseSection> CourseSections { get; } = new List<CourseSection>();
}
