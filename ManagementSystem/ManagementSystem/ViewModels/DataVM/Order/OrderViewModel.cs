using System;
using Database.Models.Order;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Order;

public class OrderViewModel : ViewModelBase
{
    private readonly OrderModel _order;

    public int Id => _order.Id;

    public DateTime CreateDate => _order.CreateDate;

    public string StatusName => _order.StatusName;

    public DateTime StatusUpdateDate => _order.StatusUpdateDate;

    public decimal Cost => _order.Cost;

    public string PaymentTypeName => _order.PaymentTypeName;

    public string BuyerEmail => _order.BuyerEmail;

    public OrderViewModel(OrderModel order)
    {
        _order = order;
    }
}