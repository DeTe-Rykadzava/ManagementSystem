using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IOrderSaleTypeService
{
    Task<ActionResultViewModel<IEnumerable<OrderSaleTypeViewModel>>> GetAllAsync();
    Task<ActionResultViewModel<OrderSaleTypeViewModel>> AddTypeAsync(string typeName);
    Task<ActionResultViewModel<bool>> RemoveTypeAsync(int typeId);
}