using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class OrderPaymentTypeService : IOrderPaymentTypeService
{
    public Task<IEnumerable<OrderPaymentTypeViewModel>> GetAllAsync()
    {
        throw new System.NotImplementedException();
    }
}