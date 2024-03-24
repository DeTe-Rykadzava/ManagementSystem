using Database.DataDatabase;

namespace Database.Models.Basket;

public class BasketProductModel
{
    public int BasketId { get; }
    
    public int ProductId { get; }

    public BasketProductModel(BasketProduct basketProduct)
    {
        BasketId = basketProduct.UserBasketId;
        ProductId = basketProduct.ProductId;
    }
}