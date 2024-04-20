using Database.Models.Core;
using Database.Models.Order;

namespace Database.Interfaces;

public interface IOrderStatusRepository
{
    Task<ActionResultModel<IEnumerable<OrderStatusModel>>> GetAllAsync();
}