using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Warehouse;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class WarehouseService : IWarehouseService
{
    public Task<WarehouseViewModel?> GetWarehouseAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<WarehouseViewModel>> GetWarehousesAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<WarehouseViewModel?> AddWarehouseAsync(string name)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> AppendProductToWarehouseAsync(int warehouseId, int productId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> UpdateProductCountInWarehouseAsync(int warehouseId, int productId, int productCount)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteProductFromWarehouseAsync(int warehouseId, int productId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteWarehouseAsync(int id)
    {
        throw new System.NotImplementedException();
    }
}