using Database.Models.Basket;

namespace Database.Interfaces;

public interface IBasketRepository
{
    Task<BasketModel?> Get(int userId);
    Task<bool> CreateBasket(int userId);
    Task<bool> AddIntoBasket(ManageProductIntoBasketModel model);
    Task<bool> RemoveFromBasket(ManageProductIntoBasketModel model);
}