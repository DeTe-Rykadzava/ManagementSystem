namespace Database.Models.Product;

public class ProductEditCountOnStockModel
{
    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public int CountProducts { get; set; }
}