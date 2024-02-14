using System.ComponentModel.DataAnnotations;

namespace Database.Models.Product;

public class ProductCreateModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [Display(Name = "Title")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    [Display(Name = "Description")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Price is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [Display(Name = "Price")]
    public decimal Cost { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    
    [Display(Name = "Images")]
    public List<byte[]> Images { get; set; }
}