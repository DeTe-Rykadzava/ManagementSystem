using Database.DataDatabase;

namespace Database.Models.Product;

public class ProductPhotoModel
{
    public int Id { get; }

    public byte[] Image { get; }

    public ProductPhotoModel(ProductPhoto photo)
    {
        Id = photo.Id;
        Image = photo.Image;
    }

    public string GetB64Image()
    {
        return Convert.ToBase64String(Image);
    }
}