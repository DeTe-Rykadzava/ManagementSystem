﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using ManagementSystem.Assets;
using ManagementSystem.Services.BasketService;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Basket;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using ManagementSystem.ViewModels.Order.Factories;
using ManagementSystem.ViewModels.Products.Factories;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Products;

public class ProductsViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "products";
    public override INavigationService RootNavManager { get; protected set; } = null!;

    // services
    private readonly IProductService _productService;
    private readonly IDialogService _dialogService;
    private readonly IUserStorageService _userStorageService;
    private readonly IEditProductFactory _editProductFactory;
    private readonly ICreateOrderVmFactory _createOrderVmFactory;
    public IUserBasketService UserBasketService { get; }

    // fields
    public ObservableCollection<ProductViewModel> Products { get; } = new ObservableCollection<ProductViewModel>();

    private bool _productsIsEmpty = true;
    public bool ProductsIsEmpty
    {
        get => _productsIsEmpty;
        set => this.RaiseAndSetIfChanged(ref _productsIsEmpty, value);
    }

    public ObservableCollection<ProductViewModel> OrderProducts { get; } = new();
    
    private bool _showGoToOrder = false;
    public bool ShowGoToOrder
    {
        get => _showGoToOrder;
        set => this.RaiseAndSetIfChanged(ref _showGoToOrder, value);
    }

    // commands
    public ICommand GoToOrderCommand { get; }
    public ICommand CreateProductCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> EditProductCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> DeleteProductCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> AddProductToUserBasketCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> AddProductToOrderCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> RemoveProductFromUserBasketCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> RemoveProductFromOrderCommand { get; }

    public ProductsViewModel(IUserStorageService userStorageService,
        IProductService productService,
        IDialogService dialogService,
        IEditProductFactory editProductFactory,
        IUserBasketService userBasketService,
        ICreateOrderVmFactory createOrderVmFactory)
    {
        _productService = productService;
        _userStorageService = userStorageService;
        _dialogService = dialogService;
        _editProductFactory = editProductFactory;
        _createOrderVmFactory = createOrderVmFactory;
        UserBasketService = userBasketService;
        CreateProductCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (_userStorageService.CurrentUser?.Role != StaticResources.AdminRoleName ||
                _userStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Sorry but you cannot create product into system",
                    icon: Icon.Error);
            }
            else
            {
                await RootNavManager.NavigateTo<CreateProductViewModel>();
            }
        });
        EditProductCommand = ReactiveCommand.CreateFromTask(async (ProductViewModel product) =>
        {
            if (_userStorageService.CurrentUser?.Role != StaticResources.AdminRoleName ||
                _userStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Sorry but you cannot edit product from system",
                    icon: Icon.Error);
            }
            else
            {
                var editRouteVm = _editProductFactory.Create(product);
                await RootNavManager.NavigateTo(editRouteVm);
            }
        });
        DeleteProductCommand = ReactiveCommand.CreateFromTask(async (ProductViewModel product) =>
        {
            if (_userStorageService.CurrentUser?.Role != StaticResources.AdminRoleName ||
                _userStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Sorry but you cannot delete product from system",
                    icon: Icon.Error);
            }
            else
            {
                var dialogResult = await dialogService.ShowPopupDialogAsync("Question",
                    "Are you sure you want to delete a product? In this case, the related orders will also be deleted, make sure that the data is saved.",
                    ButtonEnum.YesNo, Icon.Question);
                if (dialogResult == ButtonResult.No)
                    return;
                var deleteResult = await _productService.DeleteProduct(product.Id);
                if (deleteResult.IsSuccess == false)
                {
                    await dialogService.ShowPopupDialogAsync("Error",
                        $"Sorry deleted is failed. Reasons:\n\t* {string.Join("\n\t* ", deleteResult.Statuses)}",
                        icon: Icon.Error);
                    return;
                }

                await dialogService.ShowPopupDialogAsync("Success",
                    $"Success deleted", icon: Icon.Success);
                Products.Remove(product);
            }
        });
        AddProductToUserBasketCommand = ReactiveCommand.CreateFromTask(async (ProductViewModel product) =>
        {
            var result = await UserBasketService.AddToUserBasket(product);
            if (result)
                product.InUserBasket = true;
        });
        AddProductToOrderCommand = ReactiveCommand.Create((ProductViewModel product) =>
        {
            OrderProducts.Add(product);
            product.InUserOrder = true;
            ShowGoToOrder = OrderProducts.Any();
        });
        RemoveProductFromUserBasketCommand = ReactiveCommand.CreateFromTask(async (ProductViewModel product) =>
        {
            var result = await UserBasketService.RemoveFromUserBasket(product);
            if (result)
                product.InUserBasket = false;
        });
        RemoveProductFromOrderCommand = ReactiveCommand.Create((ProductViewModel product) =>
        {
            var result = OrderProducts.Remove(product);
            if (result)
                product.InUserOrder = false;
            ShowGoToOrder = OrderProducts.Any();
        });
        var canGoToOrder = this.WhenAnyValue(x => x.OrderProducts.Count, (count) => count != 0).DistinctUntilChanged();
        GoToOrderCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var vm = _createOrderVmFactory.CreateCreateOrderViewModel(OrderProducts);
            await RootNavManager.NavigateTo(vm);
        }, canGoToOrder);
    }

    public override async Task OnShowed()
    {
        Task.Run(LoadProducts);
        Dispatcher.UIThread.Invoke(new Action(() =>
        {
            OrderProducts.Clear();
        }));
        ShowGoToOrder = false;
    }

    private async Task LoadProducts()
    {
        Dispatcher.UIThread.Invoke(new Action(() =>
        {
            Products.Clear();
        }));
        var productsResult = await _productService.GetProducts();
        if(!productsResult.IsSuccess || productsResult.Value == null)
            return;
        foreach (var product in productsResult.Value)
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Products.Add(product);
                if (_userStorageService.CurrentUser == null) return;
                if (UserBasketService.Products.FirstOrDefault(x => x.Id == product.Id) != null)
                {
                    product.InUserBasket = true;
                }
            }));
        }
        ProductsIsEmpty = !Products.Any();
    }
}