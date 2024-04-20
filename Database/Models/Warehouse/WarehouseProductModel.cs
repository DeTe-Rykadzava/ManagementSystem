using Database.DataDatabase;
using Database.Models.Product;

namespace Database.Models.Warehouse;

public class WarehouseProductModel
{
    public int Id { get; }

    public int ProductId { get; }
    
    public string Title { get; }
    
    public int CountOnStock { get; }

    public WarehouseProductModel(ProductWarehouse productWarehouse)
    {
        Id = productWarehouse.Id;
        ProductId = productWarehouse.ProductId;
        Title = productWarehouse.Product.Title;
        CountOnStock = productWarehouse.Count;
    }
}