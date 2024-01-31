﻿using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class ProductCategory
{
    internal ProductCategory()
    {
        
    }
    
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}