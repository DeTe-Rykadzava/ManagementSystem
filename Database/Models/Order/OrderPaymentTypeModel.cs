using Database.DataDatabase;

namespace Database.Models.Order;

public class OrderPaymentTypeModel
{
    public int Id { get; }
    public string Type { get; }

    public OrderPaymentTypeModel(OrderPaymentType paymentType)
    {
        Id = paymentType.Id;
        Type = paymentType.Type;
    }
}