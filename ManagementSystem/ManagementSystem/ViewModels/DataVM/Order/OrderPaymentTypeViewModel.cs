using Database.Models.Order;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Order;

public class OrderPaymentTypeViewModel : ViewModelBase
{
    private readonly OrderPaymentTypeModel _orderPaymentType;

    public int Id => _orderPaymentType.Id;
    public string Type => _orderPaymentType.Type;

    public OrderPaymentTypeViewModel(OrderPaymentTypeModel orderPaymentType)
    {
        _orderPaymentType = orderPaymentType;
    }
}