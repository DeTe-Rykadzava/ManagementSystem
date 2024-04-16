using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.DataDatabase;

public partial class User
{
    internal User()
    {
        
    }
    
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public int UserInfoId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<UserBasket> UserBaskets { get; set; } = new List<UserBasket>();

    public virtual UserInfo UserInfo { get; set; } = null!;
}
