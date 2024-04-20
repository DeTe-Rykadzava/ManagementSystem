using Database.Models.Core;
using Database.Models.Product;
using Database.Models.Warehouse;

namespace Database.Interfaces;

public interface IWarehouseRepository
{
    public Task<ActionResultModel<IEnumerable<WarehouseModel>>> GetWarehousesAsync();
    public Task<ActionResultModel<WarehouseModel>> GetWarehouseAsync(int id);
    public Task<ActionResultModel<WarehouseModel>> AddWarehouseAsync(string name);
    public Task<ActionResultModel<WarehouseProductModel>> AppendProductToWarehouseAsync(WarehouseManageProductModel model);
    public Task<ActionResultModel<WarehouseProductModel>> UpdateProductCountInWarehouseAsync(WarehouseManageProductModel model);
    public Task<ActionResultModel<bool>> DeleteProductFromWarehouseAsync(WarehouseManageProductModel model);
    public Task<ActionResultModel<bool>> DeleteWarehouseAsync(int id);
}