using Database.Models.Product;
using Database.Repositories;

namespace Database.Models.Warehouse;

public class WarehouseModel
{
    public int Id { get; }

    public string Name { get; } = null!;
    
    public List<WarehouseProductModel> Products { get; }

    public WarehouseModel(DataDatabase.Warehouse warehouse)
    {
        Id = warehouse.Id;
        Name = warehouse.Name;
        Products = warehouse.ProductWarehouses.Select(s => new WarehouseProductModel(s)).ToList();
    }
}