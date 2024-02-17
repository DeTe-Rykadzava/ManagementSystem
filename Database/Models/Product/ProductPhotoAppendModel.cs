using System.ComponentModel.DataAnnotations;

namespace Database.Models.Product;

public class ProductPhotoAppendModel
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public byte[] Image { get; set; } = null!;
}