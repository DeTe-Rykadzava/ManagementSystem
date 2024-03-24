using Database.Models.Order;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Order;

public class OrderSaleTypeViewModel : ViewModelBase
{
    private readonly OrderSaleTypeModel _orderPaymentType;

    public int Id => _orderPaymentType.Id;
    public string Type => _orderPaymentType.Type;

    public OrderSaleTypeViewModel(OrderSaleTypeModel orderPaymentType)
    {
        _orderPaymentType = orderPaymentType;
    }
}