using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Database.Models.Warehouse;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.ViewModels.DataVM.Warehouse;

public class WarehouseProductViewModel : ViewModelBase
{
    private readonly WarehouseProductModel _warehouseProduct;

    public int Id => _warehouseProduct.Id;

    public int ProductId => _warehouseProduct.ProductId;

    public string Title => _warehouseProduct.Title;

    public ObservableCollection<ProductPhotoViewModel> Images { get; }

    public int CountOnStock => _warehouseProduct.CountOnStock;

    public WarehouseProductViewModel(WarehouseProductModel warehouseProduct)
    {
        _warehouseProduct = warehouseProduct;
        Images = new ObservableCollection<ProductPhotoViewModel>(_warehouseProduct.Images
            .Select(s => new ProductPhotoViewModel(s)).ToList());
    }
}