using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using DynamicData;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;
using ManagementSystem.ViewModels.Order.Factories;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Order;

public class OrdersViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "orders";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IOrderService _orderService;
    private readonly IOrderMoreInfoVmFactory _orderMoreInfoVmFactory;
    private readonly IDialogService _dialogService;

    // fields 
    public ObservableCollection<OrderViewModel> Orders { get; } = new ();

    private bool _ordersIsEmpty = true;
    public bool OrdersIsEmpty
    {
        get => _ordersIsEmpty;
        private set => this.RaiseAndSetIfChanged(ref _ordersIsEmpty, value);
    }

    // commands
    public ReactiveCommand<OrderViewModel, Unit> RemoveOrderCommand { get; }
    public ReactiveCommand<OrderViewModel, Unit> EditOrderCommand { get; }

    public OrdersViewModel(IOrderService orderService, IOrderMoreInfoVmFactory orderMoreInfoVmFactory, IDialogService dialogService)
    {
        _orderService = orderService;
        _orderMoreInfoVmFactory = orderMoreInfoVmFactory;
        _dialogService = dialogService;

        RemoveOrderCommand = ReactiveCommand.CreateFromTask(async (OrderViewModel order) =>
        {
            var dialogResult =
                await _dialogService.ShowPopupDialogAsync("Question", "Are you sure?", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            var removeResult = await _orderService.DeleteAsync(order.Id);
            if (!removeResult.IsSuccess || !removeResult.Value)
            {
                await _dialogService.ShowPopupDialogAsync("Error",
                    $"The order has not been deleted. Reasons:\n\t *{string.Join("\n\t *", removeResult.Statuses)}",
                    icon: Icon.Error);
                return;
            }
            Orders.Remove(order);
            OrdersIsEmpty = !Orders.Any();
        });
        EditOrderCommand = ReactiveCommand.CreateFromTask(async (OrderViewModel order) =>
        {
            var vm = _orderMoreInfoVmFactory.CreateOrderMoreInfoViewModel(order);
            await RootNavManager.NavigateTo(vm);
        });
    }

    public override async Task OnShowed()
    {
        Task.Run(LoadOrders);
    }

    private async Task LoadOrders()
    {
        Dispatcher.UIThread.Invoke(new Action(() =>
        {
            Orders.Clear();
            OrdersIsEmpty = true;
        }));
        var getResult = await _orderService.GetAllAsync();
        if (!getResult.IsSuccess || getResult.Value == null)
        {
            return;
        }

        foreach (var order in getResult.Value)
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Orders.Add(order);
            }));
        }

        OrdersIsEmpty = !Orders.Any();
    }
}