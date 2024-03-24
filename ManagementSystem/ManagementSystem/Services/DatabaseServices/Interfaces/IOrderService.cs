using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IOrderService
{
    public Task<OrderViewModel?> GetByIdAsync(int id);
    
    public Task<OrderViewModel?> CreateAsync(OrderCreateViewModel model);
    
    public Task<bool> UpdateStatusAsync(int orderId, int statusId);
    
    public Task<bool> DeleteAsync(int id);
}