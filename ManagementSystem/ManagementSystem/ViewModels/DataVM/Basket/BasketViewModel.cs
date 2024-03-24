using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Database.Models.Basket;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Basket;

public class BasketViewModel : ViewModelBase
{
    private readonly BasketModel _basket;

    public int Id => _basket.Id;
    public int UserId => _basket.UserId;
    public ObservableCollection<BasketProductViewModel> Products { get; }

    public BasketViewModel(BasketModel basket)
    {
        _basket = basket;
        Products = new ObservableCollection<BasketProductViewModel>(basket.Products
            .Select(s => new BasketProductViewModel(s)).ToList());
    }
}