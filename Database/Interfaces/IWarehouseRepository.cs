using Database.Models.Product;
using Database.Models.Warehouse;

namespace Database.Interfaces;

public interface IWarehouseRepository
{
    public Task<WarehouseModel?> GetWarehouseAsync(int id);
    
    public Task<IEnumerable<WarehouseModel>> GetWarehousesAsync();
    
    public Task<bool> AppendProductToWarehouseAsync(WarehouseManageProductModel model);
    
    public Task<bool> UpdateProductCountInWarehouseAsync(WarehouseManageProductModel model);
    
    public Task<bool> DeleteProductFromWarehouseAsync(WarehouseManageProductModel model);
    
    public Task<bool> DeleteWarehouseAsync(int id);
}