using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using ReactiveUI;

namespace ManagementSystem.ViewModels.DataVM.Order;

public class OrderProductCreateViewModel : ViewModelBase
{
    public ProductViewModel Product { get; }

    private int _countOfProduct = 0;
    public int CountOfProduct
    {
        get => _countOfProduct;
        set => this.RaiseAndSetIfChanged(ref _countOfProduct, value);
    }

    public OrderProductCreateViewModel(ProductViewModel product)
    {
        Product = product;
    }
}