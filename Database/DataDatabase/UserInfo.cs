using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class UserInfo
{
    internal UserInfo()
    {
        
    }
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
