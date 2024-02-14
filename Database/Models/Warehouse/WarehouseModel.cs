namespace Database.Models.Warehouse;

public class WarehouseModel
{
    public int Id { get; }

    public string Name { get; } = null!;
    
    public List<WarehouseProductModel> Products { get; }
}