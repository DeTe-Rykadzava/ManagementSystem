using Database.Models.Core;
using Database.Models.Order;

namespace Database.Interfaces;

public interface IOrderPaymentTypeRepository
{
    Task<ActionResultModel<IEnumerable<OrderPaymentTypeModel>>> GetAllAsync();
    Task<ActionResultModel<OrderPaymentTypeModel>> AddTypeAsync(string typeName);
    Task<ActionResultModel<bool>> RemoveTypeAsync(int typeId);
}