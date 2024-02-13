﻿using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class ProductPhoto
{
    internal ProductPhoto()
    {
        
    }

    public int Id { get; set; }

    public byte[] Image { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
