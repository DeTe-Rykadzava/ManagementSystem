using Database.Models.Core;
using Database.Models.Order;

namespace Database.Interfaces;

public interface IOrderRepository
{
    public Task<ActionResultModel<IEnumerable<OrderModel>>> GetAllAsync();
    public Task<ActionResultModel<IEnumerable<OrderModel>>> GetUserAllAsync(int userId);
    public Task<ActionResultModel<OrderModel>> GetByIdAsync(int id);
    public Task<ActionResultModel<OrderModel>> CreateAsync(OrderCreateModel model);
    public Task<ActionResultModel<bool>> UpdateStatusAsync(OrderEditStatusModel model);
    public Task<ActionResultModel<bool>> DeleteAsync(int id);
}