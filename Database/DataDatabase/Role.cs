using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class Role
{
    internal Role()
    {
        
    }

    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
