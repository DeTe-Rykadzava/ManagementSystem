using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class Warehouse
{
    internal Warehouse()
    {
        
    }
    
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new List<ProductWarehouse>();
}
