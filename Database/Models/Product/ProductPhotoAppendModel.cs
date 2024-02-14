namespace Database.Models.Product;

public class ProductPhotoAppendModel
{
    public int ProductId { get; set; }

    public byte[] Image { get; set; } = null!;
}