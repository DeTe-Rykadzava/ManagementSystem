using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class OrderPaymentType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
