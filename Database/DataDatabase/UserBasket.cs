using System;
using System.Collections.Generic;

namespace Database.DataDatabase;

public partial class UserBasket
{
    internal UserBasket()
    {
    }
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual BasketProduct? BasketProduct { get; set; }

    public virtual User User { get; set; } = null!;
}
