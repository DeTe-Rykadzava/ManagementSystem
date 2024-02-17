using System.ComponentModel.DataAnnotations;
using Database.Models.Product;

namespace Database.Models.Order;

public class OrderCreateModel
{
    [Required(ErrorMessage = "Buyer's email cannot be empty", AllowEmptyStrings = false)]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
    public string BuyerEmail { get; set; } = "";
    
    [Required(ErrorMessage = "Type of sale cannot be empty")]
    public int TypeSaleId { get; set; }

    [Required(ErrorMessage = "Cost cannot be empty")]
    public int PaymentTypeId { get; set; }

    public int? UserId { get; set; }

    public List<ProductModel> Products { get; set; } = new();
}