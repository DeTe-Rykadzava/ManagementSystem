using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IOrderStatusService
{
    Task<ActionResultViewModel<IEnumerable<OrderStatusViewModel>>> GetAllAsync();
}