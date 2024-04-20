using Database.DataDatabase;

namespace Database.Models.Order;

public class OrderStatusModel
{
    public int Id { get; }
    public string StatusName { get; }

    public OrderStatusModel(OrderStatus status)
    {
        Id = status.Id;
        StatusName = status.StatusName;
    }
}