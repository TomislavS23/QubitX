using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public string? RoleType { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
