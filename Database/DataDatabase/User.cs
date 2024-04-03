using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class User
{
    internal User()
    {
        
    }
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public int UserInfoId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<UserBasket> UserBaskets { get; set; } = new List<UserBasket>();

    public virtual UserInfo UserInfo { get; set; } = null!;
}
