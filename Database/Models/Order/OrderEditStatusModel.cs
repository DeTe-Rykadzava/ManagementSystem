using System.ComponentModel.DataAnnotations;

namespace Database.Models.Order;

public class OrderEditStatusModel
{
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public int StatusId { get; set; }
}