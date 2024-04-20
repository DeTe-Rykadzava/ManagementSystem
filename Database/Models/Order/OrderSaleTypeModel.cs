using Database.DataDatabase;

namespace Database.Models.Order;

public class OrderSaleTypeModel
{
    public int Id { get; }
    public string Type { get; }
    
    public OrderSaleTypeModel(OrderSaleType saleType)
    {
        Id = saleType.Id;
        Type = saleType.Type;
    }
}