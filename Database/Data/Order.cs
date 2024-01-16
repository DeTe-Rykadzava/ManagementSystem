using System;
using System.Collections.Generic;

namespace Database.Data;

public partial class Order
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public int StatusId { get; set; }

    public string? BuyerEmail { get; set; }

    public int TypeSaleId { get; set; }

    public DateTime StatusUpdateDate { get; set; }

    public decimal Cost { get; set; }

    public int PaymentTypeId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<OrderComposition> OrderCompositions { get; set; } = new List<OrderComposition>();

    public virtual OrderPaymentType PaymentType { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual OrderTypeSale TypeSale { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
