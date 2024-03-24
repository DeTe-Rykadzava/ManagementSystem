using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Warehouse;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IWarehouseService
{
    public Task<WarehouseViewModel?> GetWarehouseAsync(int id);
    
    public Task<IEnumerable<WarehouseViewModel>> GetWarehousesAsync();

    public Task<WarehouseViewModel?> AddWarehouseAsync(string name);
    
    public Task<bool> AppendProductToWarehouseAsync(int warehouseId, int productId);
    
    public Task<bool> UpdateProductCountInWarehouseAsync(int warehouseId, int productId, int productCount);
    
    public Task<bool> DeleteProductFromWarehouseAsync(int warehouseId, int productId);
    
    public Task<bool> DeleteWarehouseAsync(int id);
}