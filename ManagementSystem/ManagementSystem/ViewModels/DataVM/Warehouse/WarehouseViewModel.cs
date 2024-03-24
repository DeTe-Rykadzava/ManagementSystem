using System.Collections.ObjectModel;
using System.Linq;
using Database.Models.Warehouse;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Warehouse;

public class WarehouseViewModel : ViewModelBase
{
    private readonly WarehouseModel _warehouse;

    public int Id => _warehouse.Id;

    public string Name => _warehouse.Name;
    
    public ObservableCollection<WarehouseProductViewModel> Products { get; }

    public WarehouseViewModel(WarehouseModel warehouse)
    {
        _warehouse = warehouse;
        Products = new ObservableCollection<WarehouseProductViewModel>(_warehouse.Products
            .Select(s => new WarehouseProductViewModel(s)).ToList());
    }
}