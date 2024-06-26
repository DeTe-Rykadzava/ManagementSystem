﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ManagementSystem.Assets;
using ManagementSystem.Services;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Auth;
using ManagementSystem.ViewModels.Basket;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.User;
using ManagementSystem.ViewModels.Order;
using ManagementSystem.ViewModels.Products;
using ManagementSystem.ViewModels.Warehouse;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Splat;

namespace ManagementSystem.ViewModels.Main;

public class MainViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "main";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IDialogService _dialogService;
    public INavigationService SubNavigationService { get; }
    public IUserStorageService UserStorageService { get; }

    // Sign Commands
    public ICommand GoToSignInCommand { get; }
    public ICommand GoToSignUpCommand { get; }
    public ICommand GoToSignOutCommand { get; }
    
    // Navigation Commands
    public ICommand GoToHomeCommand { get; }
    public ICommand GoToProductsCommand { get; }
    public ICommand GoToProductCategoriesCommand { get; }
    public ICommand GoToWarehousesCommand { get; }
    public ICommand GoToUserBasketCommand { get; }
    public ICommand GoToOrdersCommand { get; }
    public ICommand GoToPaymentTypesCommand { get; }
    public ICommand GoToSaleTypesCommand { get; }

    public MainViewModel(IUserStorageService userStorageService,
                         IDialogService dialogService)

    {
        UserStorageService = userStorageService;
        _dialogService = dialogService;
            Locator.GetLocator().RegisterConstant(new NavigationService(Locator.GetLocator()),
                typeof(INavigationService), "SubNavManager");
        SubNavigationService = Locator.GetLocator().GetService<INavigationService>("SubNavManager")!;

        // sign commands
        GoToSignInCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager!.NavigateTo<SignInViewModel>();
        });
        GoToSignUpCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager!.NavigateTo<SignUpViewModel>();
        });
        GoToSignOutCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager!.NavigateTo<SignOutViewModel>();
        });

        // navigation Commands
        GoToHomeCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SubNavigationService.NavigateTo<HomeViewModel>();
        });
        GoToProductsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SubNavigationService.NavigateTo<ProductsViewModel>();
        });
        GoToProductCategoriesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (UserStorageService.CurrentUser?.Role != StaticResources.AdminRoleName || UserStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "Sorry but you cannot manipulate with product categories.", icon: Icon.Stop);
            }
            else
            {
                await SubNavigationService.NavigateTo<ProductCategoriesViewModel>();
            }
        });
        GoToWarehousesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (UserStorageService.CurrentUser == null || UserStorageService.CurrentUser?.Role != StaticResources.AdminRoleName && UserStorageService.CurrentUser?.Role != StaticResources.StorekeeperRoleName)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "Sorry but you cannot manipulate with warehouses.", icon: Icon.Stop);
            }
            else
            {
                await SubNavigationService.NavigateTo<WarehousesViewModel>();
            }
        });
        GoToUserBasketCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (UserStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "Sorry, but you can't view your basket because you're not logged in.", icon: Icon.Stop);
            }
            else
            {
                await SubNavigationService.NavigateTo<UserBasketViewModel>();
            }
        });
        GoToOrdersCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (UserStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "Sorry, but you can't view your basket because you're not logged in.", icon: Icon.Stop);
            }
            else
            {
                if (UserStorageService.CurrentUser.Role == StaticResources.AdminRoleName)
                    await SubNavigationService.NavigateTo<OrdersViewModel>();
                else
                    await SubNavigationService.NavigateTo<UserOrdersViewModel>();
            }
        });
        GoToPaymentTypesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (UserStorageService.CurrentUser?.Role != StaticResources.AdminRoleName || UserStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "Sorry but you cannot manipulate with payment types", icon: Icon.Stop);
            }
            else
            {
                await SubNavigationService.NavigateTo<OrderPaymentTypesViewModel>();
            }
        });
        GoToSaleTypesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (UserStorageService.CurrentUser?.Role != StaticResources.AdminRoleName || UserStorageService.CurrentUser == null)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "Sorry but you cannot manipulate with sale types", icon: Icon.Stop);
            }
            else
            {
                await SubNavigationService.NavigateTo<OrderSaleTypesViewModel>();
            }
        });
    }

    public override async Task OnShowed()
    {
        GoToHomeCommand.Execute(null);
    }
}