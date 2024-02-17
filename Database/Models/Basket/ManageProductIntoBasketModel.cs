using System.ComponentModel.DataAnnotations;

namespace Database.Models.Basket;

public class ManageProductIntoBasketModel
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int ProductId { get; set; }
}