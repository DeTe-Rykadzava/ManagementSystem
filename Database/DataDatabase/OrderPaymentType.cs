using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class OrderPaymentType
{
    internal OrderPaymentType()
    {
        
    }
    
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
