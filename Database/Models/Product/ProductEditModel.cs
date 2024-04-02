using System.ComponentModel.DataAnnotations;

namespace Database.Models.Product;

public class ProductEditModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Cost { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    public ProductEditModel(ProductModel product)
    {
        Id = product.Id;
        Title = product.Title;
        Description = product.Description;
        Cost = product.Cost;
        CategoryId = product.CategoryId;
    }

    public ProductEditModel() { }
}