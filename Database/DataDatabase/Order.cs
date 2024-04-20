using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.DataDatabase;

public partial class Order
{
    internal Order()
    {
        
    }
    
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public int StatusId { get; set; }

    public string BuyerEmail { get; set; }

    public int SaleTypeId { get; set; }
    
    public DateTime StatusUpdateDate { get; set; }

    public decimal Cost { get; set; }

    public int PaymentTypeId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<OrderComposition> OrderCompositions { get; set; } = new List<OrderComposition>();

    public virtual OrderPaymentType PaymentType { get; set; } = null!;

    public virtual OrderStatus Status { get; set; } = null!;

    public virtual OrderSaleType SaleType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
