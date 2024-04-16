using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.Services.BasketService;

public interface IUserBasketService
{
    public Task<bool> AddToUserBasket(ProductViewModel product);
    public Task<bool> RemoveFromUserBasket(ProductViewModel product);
}