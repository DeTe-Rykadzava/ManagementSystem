using Database.Models.Order;

namespace Database.Interfaces;

public interface IOrderRepository
{
    public Task<OrderModel?> GetByIdAsync(int id);
    
    public Task<OrderModel?> CreateAsync(OrderCreateModel model);
    
    public Task<bool> UpdateStatusAsync(OrderEditStatusModel model);
    
    public Task<bool> DeleteAsync(int id);
}