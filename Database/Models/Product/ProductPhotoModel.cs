using Database.DataDatabase;

namespace Database.Models.Product;

public class ProductPhotoModel
{
    public int Id { get; }

    public byte[] Image { get; }

    public string B64Image { get; }

    public ProductPhotoModel(ProductPhoto photo)
    {
        Id = photo.Id;
        Image = photo.Image;
        B64Image = Convert.ToBase64String(Image);
    }
}