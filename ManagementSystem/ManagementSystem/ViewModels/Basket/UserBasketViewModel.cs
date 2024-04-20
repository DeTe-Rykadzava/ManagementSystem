using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using ManagementSystem.Services.BasketService;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using ManagementSystem.ViewModels.Order.Factories;
using ManagementSystem.ViewModels.Products;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Basket;

public class UserBasketViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "basket";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    public IUserBasketService UserBasketService { get; }
    public IUserStorageService UserStorageService { get; }
    private readonly IDialogService _dialogService;
    private readonly ICreateOrderVmFactory _createOrderVmFactory;

    // fields
    public ObservableCollection<ProductViewModel> InOrderProducts { get; } = new ();

    private bool _anyProductsInOrder = false;
    public bool AnyProductsInOrder
    {
        get => _anyProductsInOrder;
        set => this.RaiseAndSetIfChanged(ref _anyProductsInOrder, value);
    }

    // commands
    public ReactiveCommand<ProductViewModel, Unit> RemoveFromBasketCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> AddToOrderCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> RemoveFromOrderCommand { get; }
    public ICommand GoToOrderCommand { get; }

    public UserBasketViewModel(IUserBasketService userBasketService, IUserStorageService userStorageService, IDialogService dialogService, ICreateOrderVmFactory createOrderVmFactory)
    {
        UserBasketService = userBasketService;
        UserStorageService = userStorageService;
        _dialogService = dialogService;
        _createOrderVmFactory = createOrderVmFactory;
        RemoveFromBasketCommand = ReactiveCommand.CreateFromTask(async (ProductViewModel product) =>
        {
            var result = await UserBasketService.RemoveFromUserBasket(product);
            if (result)
                product.InUserBasket = false;
        });
        AddToOrderCommand = ReactiveCommand.Create((ProductViewModel product) =>
        {
            InOrderProducts.Add(product);
            product.InUserOrder = true;
            AnyProductsInOrder = InOrderProducts.Any();
        });
        RemoveFromOrderCommand = ReactiveCommand.CreateFromTask(async (ProductViewModel product) =>
        {
            var result = InOrderProducts.Remove(product);
            if (result)
                product.InUserOrder = false;
            else
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Sorry, but we cannot remove product from order, it does not exits in to sequence", icon: Icon.Stop);
            }
            AnyProductsInOrder = InOrderProducts.Any();
        });
        var canGoToOrder = this.WhenAnyValue(x => x.AnyProductsInOrder).DistinctUntilChanged();
        GoToOrderCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (!InOrderProducts.Any())
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "No products in order", icon: Icon.Stop);
            }

            try
            {
                var createOrderVm = _createOrderVmFactory.CreateCreateOrderViewModel(InOrderProducts);
                await RootNavManager.NavigateTo(createOrderVm);
            }
            catch (Exception)
            {
                return;
            }
        }, canGoToOrder);
    }

    public override async Task OnShowed()
    {
        Dispatcher.UIThread.Invoke(new Action(() => 
        {
            InOrderProducts.Clear();
            AnyProductsInOrder = false;
        }));
    }
}