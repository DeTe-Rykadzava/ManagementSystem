using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Database.Models.Warehouse;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using ReactiveUI;

namespace ManagementSystem.ViewModels.DataVM.Warehouse;

public class WarehouseProductViewModel : ViewModelBase
{
    private readonly WarehouseProductModel _warehouseProduct;

    public int Id => _warehouseProduct.Id;

    public int ProductId => _warehouseProduct.ProductId;

    public string Title => _warehouseProduct.Title;

    private int _countOnStock;
    [Required]
    public int CountOnStock
    {
        get => _countOnStock;
        set => this.RaiseAndSetIfChanged(ref _countOnStock, value);
    }

    public WarehouseProductViewModel(WarehouseProductModel warehouseProduct)
    {
        _warehouseProduct = warehouseProduct;
        _countOnStock = _warehouseProduct.CountOnStock;
    }
}