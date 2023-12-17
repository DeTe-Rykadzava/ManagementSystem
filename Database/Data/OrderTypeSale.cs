using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class OrderTypeSale
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
