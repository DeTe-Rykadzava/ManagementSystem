using Database.DataDatabase;

namespace Database.Models.Basket;

public class BasketModel
{
    public int Id { get; }
    public int UserId { get; }
    public List<BasketProductModel> Products { get; }

    public BasketModel(UserBasket basket)
    {
        Id = basket.Id;
        UserId = basket.UserId;
        Products = basket.BasketProducts.Select(s => new BasketProductModel(s)).ToList();
    }
}