using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.ViewModels.Order.Factories;

public interface IOrderMoreInfoVmFactory
{
    public OrderMoreInfoViewModel CreateOrderMoreInfoViewModel(OrderViewModel order);
}