using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class OrderService : IOrderService
{
    public Task<OrderViewModel?> GetByIdAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<OrderViewModel?> CreateAsync(OrderCreateViewModel model)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> UpdateStatusAsync(int orderId, int statusId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new System.NotImplementedException();
    }
}