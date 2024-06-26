namespace Database.Models.Product;

public class ProductModel
{
    public int Id { get; }
    
    public string Title { get; }

    public string Description { get; }

    public decimal Cost { get; }
    
    public int CategoryId { get; }
    
    public string CategoryName { get; }
    
    public List<ProductPhotoModel> Images { get; }
    
    public int CountOnStocks { get; }

    
    public ProductModel(DataDatabase.Product product)
    {
        Id = product.Id;
        Title = product.Title;
        Description = product.Description;
        Cost = product.Cost;
        CategoryId = product.CategoryId;
        CategoryName = product.Category.CategoryName;
        Images = product.ProductPhotos.Select(s => new ProductPhotoModel(s)).ToList();
        CountOnStocks = product.ProductWarehouses.Sum(s => s.Count);
    }
}