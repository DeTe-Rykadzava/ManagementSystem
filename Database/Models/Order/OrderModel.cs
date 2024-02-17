namespace Database.Models.Order;

public class OrderModel
{
    public int Id { get; }

    public DateTime CreateDate { get; }
    
    public string StatusName { get; }
    
    public DateTime StatusUpdateDate { get; }
    
    public decimal Cost { get; }
    
    public string PaymentTypeName { get; }

    public string BuyerEmail { get; }

    public OrderModel(DataDatabase.Order order)
    {
        Id = order.Id;
        CreateDate = order.CreateDate;
        StatusName = order.Status.StatusName;
        StatusUpdateDate = order.StatusUpdateDate;
        Cost = order.Cost;
        PaymentTypeName = order.PaymentType.Type;
        BuyerEmail = order.BuyerEmail;
    }
}