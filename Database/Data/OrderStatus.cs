﻿using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class OrderStatus
{
    internal OrderStatus()
    {
        
    }
    
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
