using System.Collections.ObjectModel;
using System.Linq;
using Database.Models.Product;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Product;

public class ProductViewModel : ViewModelBase
{
    private readonly ProductModel _product;

    public int Id => _product.Id;

    public string Title => _product.Title;

    public string Description => _product.Description;

    public decimal Cost => _product.Cost;

    public int CategoryId => _product.CategoryId;

    public string CategoryName => _product.CategoryName;
    
    public ObservableCollection<ProductPhotoViewModel> Images { get; }

    public int CountOnStocks => _product.CountOnStocks;

    public ProductViewModel(ProductModel product)
    {
        _product = product;
        Images = new ObservableCollection<ProductPhotoViewModel>(
            _product.Images.Select(s => new ProductPhotoViewModel(s))
                .ToList());
    }

    public ProductEditViewModel ToEditViewModel()
    {
        return new ProductEditViewModel
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            CategoryId = this.CategoryId,
            Cost = (double)this.Cost
        };
    }
}