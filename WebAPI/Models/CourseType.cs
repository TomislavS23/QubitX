﻿using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class CourseType
{
    public int IdCourseType { get; set; }

    public string? CourseType1 { get; set; }

    public virtual ICollection<Course> Courses { get; } = new List<Course>();
}
