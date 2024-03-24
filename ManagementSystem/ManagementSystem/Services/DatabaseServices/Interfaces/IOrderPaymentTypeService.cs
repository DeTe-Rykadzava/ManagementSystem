using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IOrderPaymentTypeService
{
    Task<IEnumerable<OrderPaymentTypeViewModel>> GetAllAsync();
}