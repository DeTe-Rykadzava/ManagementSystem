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

    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();

    public virtual User User { get; set; } = null!;
}
