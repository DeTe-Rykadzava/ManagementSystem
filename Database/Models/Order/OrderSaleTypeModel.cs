using Database.DataDatabase;

namespace Database.Models.Order;

public class OrderSaleTypeModel
{
    public int Id { get; }
    public string Type { get; }
    
    public OrderSaleTypeModel(OrderTypeSale typeSale)
    {
        Id = typeSale.Id;
        Type = typeSale.TypeName;
    }
}