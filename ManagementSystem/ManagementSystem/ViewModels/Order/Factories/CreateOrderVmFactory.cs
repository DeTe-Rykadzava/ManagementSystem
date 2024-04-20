using System.Collections.Generic;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.ViewModels.Order.Factories;

public class CreateOrderVmFactory : ICreateOrderVmFactory
{
    private readonly IOrderService _orderService;
    private readonly IOrderSaleTypeService _orderSaleTypeService;
    private readonly IOrderPaymentTypeService _orderPaymentTypeService;
    private readonly IUserStorageService _userStorageService;
    private readonly IDialogService _dialogService;

    public CreateOrderVmFactory(IOrderService orderService, IOrderSaleTypeService orderSaleTypeService,
        IOrderPaymentTypeService orderPaymentTypeService, IUserStorageService userStorageService, IDialogService dialogService) =>
        (_orderService, _orderSaleTypeService, _orderPaymentTypeService, _userStorageService, _dialogService) =
        (orderService, orderSaleTypeService, orderPaymentTypeService, userStorageService, dialogService);
    
    public CreateOrderViewModel CreateCreateOrderViewModel(IEnumerable<ProductViewModel> products)
    {
        return new CreateOrderViewModel(_orderService, _orderSaleTypeService, _orderPaymentTypeService,
            _userStorageService, _dialogService, products);
    }
}