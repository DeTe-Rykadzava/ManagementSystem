using Database.Models.Product;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Product;

public class ProductViewModelMinimalData : ViewModelBase
{
    private readonly ProductModelMinimalData _product;

    public int Id => _product.Id;

    public string Title => _product.Title;

    public int CategoryId => _product.CategoryId;

    public string CategoryName => _product.CategoryName;

    public ProductViewModelMinimalData(ProductModelMinimalData product)
    {
        _product = product;
    }
}