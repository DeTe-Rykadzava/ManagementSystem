using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class OrderTypeSale
{
    internal OrderTypeSale()
    {
        
    }
    
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
