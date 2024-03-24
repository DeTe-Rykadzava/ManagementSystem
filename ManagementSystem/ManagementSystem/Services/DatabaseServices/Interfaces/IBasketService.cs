using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Basket;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IBasketService
{
    Task<BasketViewModel?> Get(int userId);
    Task<bool> CreateBasket(int userId);
    Task<bool> AddIntoBasket(int userId, int productId);
    Task<bool> RemoveFromBasket(int userId, int productId);
}