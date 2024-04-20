using Database.Models.Core;
using Database.Models.Order;

namespace Database.Interfaces;

public interface IOrderSaleTypeRepository
{
    Task<ActionResultModel<IEnumerable<OrderSaleTypeModel>>> GetAllAsync();
    Task<ActionResultModel<OrderSaleTypeModel>> AddTypeAsync(string typeName);
    Task<ActionResultModel<bool>> RemoveTypeAsync(int typeId);
}