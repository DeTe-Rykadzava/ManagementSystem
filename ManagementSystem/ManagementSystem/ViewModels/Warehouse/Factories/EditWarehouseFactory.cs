using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.ViewModels.DataVM.Warehouse;

namespace ManagementSystem.ViewModels.Warehouse.Factories;

public class EditWarehouseFactory : IEditWarehouseFactory
{
    private readonly IWarehouseService _warehouseService;
    private readonly IProductService _productService;
    private readonly IDialogService _dialogService;
    
    public EditWarehouseFactory(IWarehouseService warehouseService, IProductService productService, IDialogService dialogService)
    {
        _warehouseService = warehouseService;
        _productService = productService;
        _dialogService = dialogService;
    }
    
    public EditWarehouseViewModel Create(WarehouseViewModel warehouse)
    {
        return new EditWarehouseViewModel(_warehouseService, _productService, _dialogService, warehouse);
    }
}