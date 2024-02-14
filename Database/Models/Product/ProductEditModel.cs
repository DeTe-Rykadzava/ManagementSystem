namespace Database.Models.Product;

public class ProductEditModel
{
    public int Id { get; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public decimal Cost { get; set; }
    
    public int CategoryId { get; set; }
    
    public ProductEditModel(ProductModel product)
    {
        Id = product.Id;
        Title = product.Title;
        Description = product.Description;
        Cost = product.Cost;
        CategoryId = product.CategoryId;
    }
}