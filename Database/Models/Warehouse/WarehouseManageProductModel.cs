using System.ComponentModel.DataAnnotations;

namespace Database.Models.Warehouse;

public class WarehouseManageProductModel
{
    [Required]
    public int WarehouseId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int CountProducts { get; set; }
}