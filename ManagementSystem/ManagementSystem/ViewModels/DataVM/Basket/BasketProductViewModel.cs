using Database.Models.Basket;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Basket;

public class BasketProductViewModel : ViewModelBase
{
    private readonly BasketProductModel _basketProduct;

    public int BasketId => _basketProduct.BasketId;

    public int ProductId => _basketProduct.ProductId;

    public BasketProductViewModel(BasketProductModel basketProduct)
    {
        _basketProduct = basketProduct;
    }
}