using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class UserCourse
{
    public int IdUserCourse { get; set; }

    public int? IdUser { get; set; }

    public int? IdCourse { get; set; }

    public virtual Course? IdCourseNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
