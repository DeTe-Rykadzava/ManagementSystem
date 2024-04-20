using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Warehouse;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IWarehouseService
{
    public Task<ActionResultViewModel<WarehouseViewModel>> GetWarehouseAsync(int id);
    
    public Task<ActionResultViewModel<IEnumerable<WarehouseViewModel>>> GetWarehousesAsync();

    public Task<ActionResultViewModel<WarehouseViewModel>> AddWarehouseAsync(string name);
    
    public Task<ActionResultViewModel<WarehouseProductViewModel>> AppendProductToWarehouseAsync(int warehouseId, int productId);
    
    public Task<ActionResultViewModel<WarehouseProductViewModel>> UpdateProductCountInWarehouseAsync(int warehouseId, int productId, int productCount);
    
    public Task<ActionResultViewModel<bool>> DeleteProductFromWarehouseAsync(int warehouseId, int productId);
    
    public Task<ActionResultViewModel<bool>> DeleteWarehouseAsync(int id);
}