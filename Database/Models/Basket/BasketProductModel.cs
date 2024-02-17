using Database.DataDatabase;

namespace Database.Models.Basket;

public class BasketProductModel
{
    public int BasketId { get; set; }
    
    public int ProductId { get; set; }

    public BasketProductModel(BasketProduct basketProduct)
    {
        BasketId = basketProduct.UserBasketId;
        ProductId = basketProduct.ProductId;
    }
}