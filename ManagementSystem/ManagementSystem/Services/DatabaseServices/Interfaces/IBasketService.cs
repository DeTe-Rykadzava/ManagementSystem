using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Basket;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IBasketService
{
    Task<ActionResultViewModel<BasketViewModel>> Get(int userId);
    Task<ActionResultViewModel<BasketViewModel>> CreateBasket(int userId);
    Task<bool> AddIntoBasket(int userId, int productId);
    Task<bool> RemoveFromBasket(int userId, int productId);
}