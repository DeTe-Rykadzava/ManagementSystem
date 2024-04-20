using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Warehouse;

namespace ManagementSystem.ViewModels.Warehouse.Factories;

public interface IEditWarehouseFactory
{
    public EditWarehouseViewModel Create(WarehouseViewModel warehouse);
}