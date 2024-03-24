using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Database.Context;
using Database.Interfaces;
using Database.Repositories;
using ManagementSystem.Services;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DatabaseServices.Services;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Auth;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.Main;
using ManagementSystem.Views;
using ManagementSystem.Views.Auth;
using ManagementSystem.Views.Main;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;
using HomeView = ManagementSystem.Views.Main.HomeView;
using ILogger = Splat.ILogger;

namespace ManagementSystem.ViewModels;

public class AppViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; }

    public AppViewModel()
    {
        // register database
        Locator.GetLocator().RegisterConstant(Database.DatabaseCore.DatabaseSettings.GetDbContext);
        
        // register database repositories
        Locator.GetLocator().Register<IUserRepository>(            () => new UserRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IRoleRepository>(            () => new RoleRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IBasketRepository>(          () => new BasketRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IOrderPaymentTypeRepository>(() => new OrderPaymentTypeRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IOrderRepository>(           () => new OrderRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IOrderSaleTypeRepository>(   () => new OrderSaleTypeRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IProductRepository>(         () => new ProductRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IWarehouseRepository>(       () => new WarehouseRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));

        // register data services
        Locator.GetLocator().Register<IBasketService>(() => new BasketService());
        Locator.GetLocator().Register<IOrderPaymentTypeService>(() => new OrderPaymentTypeService());
        Locator.GetLocator().Register<IOrderSaleTypeService>(() => new OrderSaleTypeService());
        Locator.GetLocator().Register<IOrderService>(() => new OrderService());
        Locator.GetLocator().Register<IProductService>(() => new ProductService());
        Locator.GetLocator().Register<IRoleService>(() => new RoleService());
        Locator.GetLocator().Register<IUserService>(() => new UserService());
        Locator.GetLocator().Register<IWarehouseService>(() => new WarehouseService());
        
        // register program services
        Locator.GetLocator().RegisterConstant(new NavigationService(), typeof(INavigationService), "MainNavManager");
        Locator.GetLocator().RegisterConstant(new UserStorageService(), typeof(IUserStorageService));
        
        // register ViewModels
        Locator.GetLocator().RegisterConstant<MainViewModel>(new MainViewModel(Locator.GetLocator().GetService<IUserStorageService>()!));
        Locator.GetLocator().Register<SignInViewModel>(() => new SignInViewModel(
            Locator.GetLocator().GetService<IUserStorageService>()!,
                  Locator.GetLocator().GetService<IUserService>()!));
        Locator.GetLocator().Register<SignUpViewModel>(() => new SignUpViewModel());
        Locator.GetLocator().Register<HomeViewModel>(() => new HomeViewModel());

        // register Views
        Locator.GetLocator().Register(() => new MainView(), typeof(IViewFor<MainViewModel>));
        Locator.GetLocator().Register(() => new HomeView(), typeof(IViewFor<HomeViewModel>));
        Locator.GetLocator().Register(() => new SignInView(), typeof(IViewFor<SignInViewModel>));
        Locator.GetLocator().Register(() => new SignUpView(), typeof(IViewFor<SignUpViewModel>));
        
        NavigationService = Locator.GetLocator().GetService<INavigationService>("MainNavManager")!;
        
        Task.Run(LoadMainView);
    }

    private async void LoadMainView()
    {
        await NavigationService.NavigateTo<MainViewModel>();
    }
}