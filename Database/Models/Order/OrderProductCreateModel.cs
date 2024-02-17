namespace Database.Models.Order;

public class OrderProductCreateModel
{
    public int ProductId { get; }

    public int ProductsCount { get; }

    public OrderProductCreateModel(int productId, int productsCount)
    {
        ProductId = productId;
        ProductsCount = productsCount;
    }
}