using Database.DataDatabase;

namespace Database.Models.Product;

public class ProductPhotoModel
{
    public int Id { get; set; }

    public byte[] Image { get; set; }

    public string B64Image { get; set; }

    public ProductPhotoModel(ProductPhoto photo)
    {
        Id = photo.Id;
        Image = photo.Image;
        B64Image = Convert.ToBase64String(Image);
    }
}