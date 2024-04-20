using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IOrderPaymentTypeService
{
    Task<ActionResultViewModel<IEnumerable<OrderPaymentTypeViewModel>>> GetAllAsync();
    Task<ActionResultViewModel<OrderPaymentTypeViewModel>> AddTypeAsync(string typeName);
    Task<ActionResultViewModel<bool>> RemoveTypeAsync(int typeId);
}