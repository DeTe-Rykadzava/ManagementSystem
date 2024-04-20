using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Order;

public class UserOrdersViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "user_orders";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IOrderService _orderService;
    private readonly IUserStorageService _userStorageService;
    private readonly IDialogService _dialogService;
    
    // fields
    public ObservableCollection<OrderViewModel> UserOrders { get; } = new();

    private bool _userOrdersIsEmpty = true;
    public bool UserOrdersIsEmpty
    {
        get => _userOrdersIsEmpty;
        private set => this.RaiseAndSetIfChanged(ref _userOrdersIsEmpty, value);
    }

    public UserOrdersViewModel(IOrderService orderService, IUserStorageService userStorageService, IDialogService dialogService)
    {
        _orderService = orderService;
        _userStorageService = userStorageService;
        _dialogService = dialogService;
    }

    public override async Task OnShowed()
    {
        Task.Run(LoadUserOrders);
    }

    private async Task LoadUserOrders()
    {
        Dispatcher.UIThread.Invoke(new Action(() =>
        {
            UserOrders.Clear();
        }));
        if (_userStorageService.CurrentUser == null)
        {
            UserOrdersIsEmpty = true;
            return;
        }
        var getResult = await _orderService.GetUserAllAsync(_userStorageService.CurrentUser.Id);
        if (!getResult.IsSuccess || getResult.Value == null)
        {
            Dispatcher.UIThread.Invoke(new Action(async () =>
            {
                await _dialogService.ShowPopupDialogAsync("Error", "User's order data could not be uploaded",
                    icon: Icon.Error);
            }));
            return;
        }

        foreach (var order in getResult.Value)
        { 
            Dispatcher.UIThread.Invoke(new Action(async () =>
            {
                UserOrders.Add(order);
            }));
        }

        UserOrdersIsEmpty = !UserOrders.Any();
    }
}