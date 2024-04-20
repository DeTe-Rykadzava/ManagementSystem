using Database.Models.Order;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Order;

public class OrderStatusViewModel : ViewModelBase
{
    private OrderStatusModel _status;

    public int Id => _status.Id;
    public string StatusName => _status.StatusName;

    public OrderStatusViewModel(OrderStatusModel status)
    {
        _status = status;
    }
}