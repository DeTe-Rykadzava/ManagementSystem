using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Database.Context;
using Database.Interfaces;
using Database.Repositories;
using ManagementSystem.Services;
using ManagementSystem.ViewModels.Auth;
using ManagementSystem.ViewModels.Core;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;
using ILogger = Splat.ILogger;

namespace ManagementSystem.ViewModels;

public class AppViewModel : ViewModelBase
{
    public string? UrlPathSegment { get; } = "app";
    
    public NavigationService NavigationService { get; }

    public AppViewModel()
    {
        // register database
        Locator.GetLocator().RegisterConstant(Database.DatabaseCore.DatabaseSettings.GetDbContext);
        
        // register database services
        Locator.GetLocator().Register<IUserRepository>(            () => new UserRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IRoleRepository>(            () => new RoleRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IBasketRepository>(          () => new BasketRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IOrderPaymentTypeRepository>(() => new OrderPaymentTypeRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IOrderRepository>(           () => new OrderRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IOrderSaleTypeRepository>(   () => new OrderSaleTypeRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IProductRepository>(         () => new ProductRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));
        Locator.GetLocator().Register<IWarehouseRepository>(       () => new WarehouseRepository(Locator.GetLocator().GetService<IManagementSystemDatabaseContext>()!));

        // register services
        Locator.GetLocator().RegisterConstant(new NavigationService(), typeof(NavigationService), "MainNavManager");
        
        // register ViewModels
        Locator.GetLocator().RegisterConstant<MainViewModel>(new MainViewModel());
        Locator.GetLocator().Register<LoginViewModel>(() => new LoginViewModel());
        Locator.GetLocator().Register<RegistrationViewModel>(() => new RegistrationViewModel());

        NavigationService = Locator.GetLocator().GetService<NavigationService>("MainNavManager")!;
        
        Task.Run(LoadMainView);
    }

    private async void LoadMainView()
    {
        await NavigationService.NavigateTo<MainViewModel>();
    }
}