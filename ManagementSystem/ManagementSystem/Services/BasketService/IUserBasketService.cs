using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.Services.BasketService;

public interface IUserBasketService
{
    public ObservableCollection<ProductViewModel> Products { get; }
    public bool UserBasketProductsIsEmpty { get; }
    public Task<bool> AddToUserBasket(ProductViewModel product);
    public Task<bool> RemoveFromUserBasket(ProductViewModel product);
}