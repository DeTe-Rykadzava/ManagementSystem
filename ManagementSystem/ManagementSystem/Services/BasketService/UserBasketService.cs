using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.DataVM.Product;
using ManagementSystem.ViewModels.DataVM.User;

namespace ManagementSystem.Services.BasketService;

public class UserBasketService : IUserBasketService
{
    private readonly IUserStorageService _userStorageService;
    private readonly IBasketService _basketService;

    public ObservableCollection<ProductViewModel> Products { get; } = new(); 

    public UserBasketService(IUserStorageService userStorageService, IBasketService basketService)
    {
        _userStorageService = userStorageService;
        _basketService = basketService;
        _userStorageService.PropertyChanged += (sender, args) =>
        {
            if (sender is UserViewModel user)
            {
            }
        };
    }

    public async Task<bool> AddToUserBasket(ProductViewModel product)
    {
        if(_userStorageService.CurrentUser == null)
            return false;
        
        Products.Add(product);
        return true;
    }

    public async Task<bool> RemoveFromUserBasket(ProductViewModel product)
    {
        if(_userStorageService.CurrentUser == null)
            return false;
        
        Products.Remove(product);
        return true;
    }
}