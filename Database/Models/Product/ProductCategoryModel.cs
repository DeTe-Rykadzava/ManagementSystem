using Database.DataDatabase;

namespace Database.Models.Product;

public class ProductCategoryModel
{
    public int Id { get; set; }
    public string CategoryName { get; }

    public ProductCategoryModel(ProductCategory category)
    {
        Id = category.Id;
        CategoryName = category.CategoryName;
    }
    
}