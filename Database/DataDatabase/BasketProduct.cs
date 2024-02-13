using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class BasketProduct
{
    internal BasketProduct()
    {
    }

    public int UserBasketId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual UserBasket UserBasket { get; set; } = null!;
}
