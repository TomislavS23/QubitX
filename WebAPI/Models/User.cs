using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Username { get; set; }

    public string? HashedPassword { get; set; }

    public int? IdRole { get; set; }

    public virtual ICollection<Course> Courses { get; } = new List<Course>();

    public virtual Role? IdRoleNavigation { get; set; }

    public virtual ICollection<UserCourse> UserCourses { get; } = new List<UserCourse>();
}
