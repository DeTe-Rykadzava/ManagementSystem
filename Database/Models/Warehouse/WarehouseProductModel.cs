using Database.Models.Product;

namespace Database.Models.Warehouse;

public class WarehouseProductModel
{
    public int Id { get; }
    
    public string Title { get; }

    public string Description { get; }

    public decimal Cost { get; }
    
    public int CategoryId { get; }
    
    public string CategoryName { get; }
    
    public List<ProductPhotoModel> Images { get; }

    public int CountOnStock { get; }
}