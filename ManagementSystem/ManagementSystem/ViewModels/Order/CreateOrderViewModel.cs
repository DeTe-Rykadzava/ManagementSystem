using System;
using System.Collections.Generic;
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
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;
using ManagementSystem.ViewModels.DataVM.Product;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Order;

public class CreateOrderViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "create_order";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IOrderService _orderService;
    private readonly IOrderSaleTypeService _orderSaleTypeService;
    private readonly IOrderPaymentTypeService _orderPaymentTypeService;
    private readonly IDialogService _dialogService;
    private readonly IUserStorageService _userStorageService;

    // fields
    public OrderCreateViewModel Model { get; }

    private string? _status;
    public string? Status
    {
        get => _status;
        private set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public ObservableCollection<OrderSaleTypeViewModel> SaleTypes { get; } = new();
    
    public ObservableCollection<OrderPaymentTypeViewModel> PaymentTypes { get; } = new();

    // commands
    public ReactiveCommand<OrderProductCreateViewModel, Unit> RemoveFromOrder { get; }
    public ICommand SaveCommand { get; }
    public ICommand CanselCommand { get; }

    public CreateOrderViewModel(IOrderService orderService, IOrderSaleTypeService orderSaleTypeService, IOrderPaymentTypeService orderPaymentTypeService, IUserStorageService userStorageService, IDialogService dialogService, IEnumerable<ProductViewModel> products)
    {
        _orderService = orderService;
        _orderSaleTypeService = orderSaleTypeService;
        _orderPaymentTypeService = orderPaymentTypeService;
        _userStorageService = userStorageService;
        _dialogService = dialogService;
        Model = new OrderCreateViewModel();
        Model.UserId = _userStorageService.CurrentUser?.Id;
        Model.Products = new (products.Select(s => new OrderProductCreateViewModel(s)).ToList());
        SaveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Status = null;
            var validateResult = await Model.CheckValid();
            if (!validateResult.IsSuccess || !validateResult.Value)
            {
                Status =
                    $"Error when checking the correctness of the entered data, check the correctness.The reasons for the error:\n\t *{string.Join("\n\t *", validateResult.Statuses)}";
            }
            else
            {
                var saveResult = await _orderService.CreateAsync(Model);
                if (!saveResult.IsSuccess || saveResult.Value == null)
                {
                    Status =
                        $"Error when creating an order,The reasons for the error:\n\t *{string.Join("\n\t *", saveResult.Statuses)}";
                    await _dialogService.ShowPopupDialogAsync("Error", Status, icon: Icon.Error);
                    return;
                }

                await _dialogService.ShowPopupDialogAsync("Success", "Success", icon: Icon.Success);
                await RootNavManager.GoBack();
            }
        });

        RemoveFromOrder = ReactiveCommand.CreateFromTask(async (OrderProductCreateViewModel product) =>
        {
            var dialogResult =
                await _dialogService.ShowPopupDialogAsync("Question", "Are you sure?", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            Model.Products.Remove(product);
        });
        
        CanselCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var dialogResult =
                await _dialogService.ShowPopupDialogAsync("Question", "Are you sure want to cansel?", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            await RootNavManager.GoBack();
        });
    }

    public override async Task OnShowed()
    {
        Task.Run(LoadData);
        
    }

    private async void LoadData()
    {
        await Task.Run(LoadSaleTypes);
        await Task.Run(LoadPaymentTypes);
    }

    private async Task LoadSaleTypes()
    {
        var getResult = await _orderSaleTypeService.GetAllAsync();
        if (!getResult.IsSuccess || getResult.Value == null)
        {
            Dispatcher.UIThread.Invoke(new Action(async () =>
            {
                await _dialogService.ShowPopupDialogAsync("Error",
                    "The required data could not be downloaded, please go back to the page.", icon: Icon.Error);
            }));
            return;
        }

        foreach (var saleType in getResult.Value)
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                SaleTypes.Add(saleType);
            }));
        }
    }
    
    private async Task LoadPaymentTypes()
    {
        var getResult = await _orderPaymentTypeService.GetAllAsync();
        if (!getResult.IsSuccess || getResult.Value == null)
        {
            Dispatcher.UIThread.Invoke(new Action(async () =>
            {
                await _dialogService.ShowPopupDialogAsync("Error", "The required data could not be downloaded, please go back to the page.", icon: Icon.Error);
            }));
            return;
        }

        foreach (var paymentType in getResult.Value)
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                PaymentTypes.Add(paymentType);
            }));
        }
    }
}