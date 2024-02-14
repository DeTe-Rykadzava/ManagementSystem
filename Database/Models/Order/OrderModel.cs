namespace Database.Models.Order;

public class OrderModel
{
    public int Id { get; }

    public DateTime CreateDate { get; }
    
    public string StatusName { get; }
    
    public DateTime StatusUpdateDate { get; }
    
    public decimal Cost { get; }
    
    public int PaymentTypeId { get; }
    
    public int UserId { get; }
}