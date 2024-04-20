using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Order;

public class OrderMoreInfoViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "order-info";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IOrderService _orderService;
    private readonly IOrderStatusService _orderStatusService;
    private readonly IDialogService _dialogService;
    
    // fields
    public OrderViewModel Order { get; }

    private OrderStatusViewModel? _orderStatus;
    public OrderStatusViewModel? OrderStatus
    {
        get => _orderStatus;
        set => this.RaiseAndSetIfChanged(ref _orderStatus, value);
    }

    public ObservableCollection<OrderStatusViewModel> Statuses { get; } = new ();

    private string? _status;
    public string? Status
    {
        get => _status;
        private set => this.RaiseAndSetIfChanged(ref _status, value);
    }
    
    // commands
    public ICommand UpdateStatusCommand { get; }
    public ICommand CanselCommand { get; }

    public OrderMoreInfoViewModel(IOrderService orderService, IOrderStatusService orderStatusService, IDialogService dialogService, OrderViewModel order)
    {
        _orderService = orderService;
        _orderStatusService = orderStatusService;
        _dialogService = dialogService;
        Order = order;
        UpdateStatusCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var dialogResult = await _dialogService.ShowPopupDialogAsync("Question", "Are you sure?", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            if (OrderStatus == null)
            {
                Status = "The status is not specified";
                await _dialogService.ShowPopupDialogAsync("Stop", Status, icon: Icon.Stop);
                return;
            }

            var updateResult = await _orderService.UpdateStatusAsync(Order.Id, OrderStatus.Id);
            if (!updateResult.IsSuccess || !updateResult.Value)
            {
                Status = $"The data could not be updated. Reasons:\n\t *{string.Join("\n\t *", updateResult.Statuses)}";
                await _dialogService.ShowPopupDialogAsync("Error",
                    Status,
                    icon: Icon.Error);
                return;
            }
            
            await RootNavManager.GoBack();
        });
        CanselCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
    }

    public override async Task OnShowed()
    {
        Task.Run(LoadOrderStatuses);
    }

    private async Task LoadOrderStatuses()
    {
        var getResult = await _orderStatusService.GetAllAsync();
        if (!getResult.IsSuccess || getResult.Value == null)
        {
            await _dialogService.ShowPopupDialogAsync("Error",
                $"The necessary data could not be obtained, and there is no way to update the entity. Reasons:\n\t *{string.Join("\n\t *", getResult.Statuses)}",
                icon: Icon.Error);
            return;
        }

        foreach (var status in getResult.Value)
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Statuses.Add(status);
                if (status.StatusName == Order.StatusName)
                    OrderStatus = status;
            }));
        }
    }
}