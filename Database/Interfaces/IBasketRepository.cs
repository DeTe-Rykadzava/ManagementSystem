using Database.Models.Basket;
using Database.Models.Core;

namespace Database.Interfaces;

public interface IBasketRepository
{
    Task<ActionResultModel<BasketModel>> Get(int userId);
    Task<ActionResultModel<BasketModel>> CreateBasket(int userId);
    Task<bool> AddIntoBasket(ManageProductIntoBasketModel model);
    Task<bool> RemoveFromBasket(ManageProductIntoBasketModel model);
}