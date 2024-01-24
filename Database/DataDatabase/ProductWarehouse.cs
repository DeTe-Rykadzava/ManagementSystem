using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class ProductWarehouse
{
    internal ProductWarehouse()
    {
        
    }
    
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public int Count { get; set; }

    public int CountReserved { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
