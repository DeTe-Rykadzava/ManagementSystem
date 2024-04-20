namespace Database.Models.Product;

public class ProductModelMinimalData
{
    public int Id { get; }
    
    public string Title { get; }
    
    public int CategoryId { get; }
    
    public string CategoryName { get; }

    public decimal Cost { get; }

    public ProductModelMinimalData(DataDatabase.Product product)
    {
        Id = product.Id;
        Title = product.Title;
        CategoryId = product.CategoryId;
        CategoryName = product.Category.CategoryName;
        Cost = product.Cost;
    }
}