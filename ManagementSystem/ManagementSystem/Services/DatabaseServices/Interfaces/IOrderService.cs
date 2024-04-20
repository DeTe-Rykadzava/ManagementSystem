using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IOrderService
{
    public Task<ActionResultViewModel<IEnumerable<OrderViewModel>>> GetAllAsync();
    public Task<ActionResultViewModel<IEnumerable<OrderViewModel>>> GetUserAllAsync(int userId);
    public Task<ActionResultViewModel<OrderViewModel>> GetByIdAsync(int id);
    public Task<ActionResultViewModel<OrderViewModel>> CreateAsync(OrderCreateViewModel model);
    public Task<ActionResultViewModel<bool>> UpdateStatusAsync(int orderId, int statusId);
    public Task<ActionResultViewModel<bool>> DeleteAsync(int id);
}