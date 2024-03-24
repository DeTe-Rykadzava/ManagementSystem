using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Basket;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class BasketService : IBasketService
{
    public Task<BasketViewModel?> Get(int userId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> CreateBasket(int userId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> AddIntoBasket(int userId, int productId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> RemoveFromBasket(int userId, int productId)
    {
        throw new System.NotImplementedException();
    }
}