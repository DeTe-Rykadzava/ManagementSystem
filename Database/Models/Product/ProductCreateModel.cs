namespace Database.Models.Product;

public class ProductCreateModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Cost { get; set; }

    public int CategoryId { get; set; }

    public List<byte[]> Images { get; set; } = new();
}