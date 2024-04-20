using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;

namespace ManagementSystem.ViewModels.Order.Factories;

public class OrderMoreInfoVmFactory : IOrderMoreInfoVmFactory
{
    private readonly IOrderService _orderService;
    private readonly IOrderStatusService _orderStatusService;
    private readonly IDialogService _dialogService;

    public OrderMoreInfoVmFactory(IOrderService orderService, IOrderStatusService orderStatusService, IDialogService dialogService) =>
        (_orderService, _orderStatusService, _dialogService) = (orderService, orderStatusService, dialogService);
    
    public OrderMoreInfoViewModel CreateOrderMoreInfoViewModel(OrderViewModel order)
    {
        return new OrderMoreInfoViewModel(_orderService, _orderStatusService, _dialogService, order);
    }
}