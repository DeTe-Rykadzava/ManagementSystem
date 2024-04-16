﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Database.Context;
using Database.Interfaces;
using Database.Repositories;
using ManagementSystem.Services.BasketService;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DatabaseServices.Services;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.Logger;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.Storage;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Auth;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.Main;
using ManagementSystem.ViewModels.Products;
using ManagementSystem.ViewModels.Products.Factories;
using ManagementSystem.Views.Auth;
using ManagementSystem.Views.Main;
using ManagementSystem.Views.Products;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Splat;
using HomeView = ManagementSystem.Views.Main.HomeView;

namespace ManagementSystem.ViewModels;

public class AppViewModel : ViewModelBase
{
    private INavigationService? _navigationService;
    public INavigationService? NavigationService
    {
        get => _navigationService;
        private set => this.RaiseAndSetIfChanged(ref _navigationService, value);
    } 
    
    private readonly IDependencyResolver _locator;
    private ILogger<AppViewModel>? _logger = null;
    
    private string _status = string.Empty;

    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public Interaction<Unit, Unit> RemoveLoadPanel { get; }

    public AppViewModel()
    {
        _locator = Locator.GetLocator();
        RemoveLoadPanel = new Interaction<Unit, Unit>();
        
        Task.Run(LoadApp);
    }

    private async Task LoadApp()
    {
        try
        {
            var lifeRuntime = Application.Current?.ApplicationLifetime;
            if (lifeRuntime == null)
            {
                Status = "The application lifetime cannot be obtained, please restart the program and contact your administrator.";
                return;
            }
            
            // register logger
            Status = "Start register application logger";
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _locator.RegisterConstant( new ProgramLoggerFactory(loggerFactory), typeof(IProgramLoggerFactory));
            var programLoggerFactory = _locator.GetService<IProgramLoggerFactory>()!;
            _logger = programLoggerFactory.CreateLogger<AppViewModel>();
            _logger.LogInformation("App starting");
            Status = "Successful register application logger";
            
            // register database
            _logger.LogInformation("Register database");
            Status = "Start register database";
            _locator.RegisterConstant(Database.DatabaseCore.DatabaseSettings.GetDbContext(), typeof(IManagementSystemDatabaseContext));
            Status = "Successful register database";
            _logger.LogInformation("Successful register database");
            
            // register database repositories
            _logger.LogInformation("Register database repositories");
            Status = "Start register database repository";        
            _locator.RegisterConstant<IUserRepository>(            new UserRepository(            _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<UserRepository>()));
            _logger.LogInformation("IUserRepository registered");
            Status = "Successful register 1/9 database repository";
            _locator.RegisterConstant<IRoleRepository>(            new RoleRepository(            _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<RoleRepository>()));
            _logger.LogInformation("IRoleRepository registered");
            Status = "Successful register 2/9 database repository";
            _locator.RegisterConstant<IBasketRepository>(          new BasketRepository(          _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<BasketRepository>()));
            _logger.LogInformation("IBasketRepository registered");
            Status = "Successful register 3/9 database repository";
            _locator.RegisterConstant<IOrderPaymentTypeRepository>(new OrderPaymentTypeRepository(_locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<OrderPaymentTypeRepository>()));
            _logger.LogInformation("IOrderPaymentTypeRepository registered");
            Status = "Successful register 4/9 database repository";
            _locator.RegisterConstant<IOrderRepository>(           new OrderRepository(           _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<OrderRepository>()));
            _logger.LogInformation("IOrderRepository registered");
            Status = "Successful register 5/9 database repository";
            _locator.RegisterConstant<IOrderSaleTypeRepository>(   new OrderSaleTypeRepository(   _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<OrderSaleTypeRepository>()));
            _logger.LogInformation("IOrderSaleTypeRepository registered");
            Status = "Successful register 6/9 database repository";
            _locator.RegisterConstant<IProductRepository>(         new ProductRepository(         _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<ProductRepository>()));
            _logger.LogInformation("IProductRepository registered");
            Status = "Successful register 7/9 database repository";
            _locator.RegisterConstant<IProductCategoryRepository>( new ProductCategoryRepository( _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<ProductCategoryRepository>()));
            _logger.LogInformation("IProductRepository registered");
            Status = "Successful register 8/9 database repository";
            _locator.RegisterConstant<IWarehouseRepository>(       new WarehouseRepository(       _locator.GetService<IManagementSystemDatabaseContext>()!, programLoggerFactory.CreateLogger<WarehouseRepository>()));
            _logger.LogInformation("IWarehouseRepository registered");
            Status = "Successful register 9/9 database repository";
            Status = "Successful register all database repository";
            _logger.LogInformation("Successful register database repositories");
    
            // register database services
            _logger.LogInformation("Register database services");
            Status = "Start register database services";  
            _locator.RegisterConstant<IBasketService>(           new BasketService(_locator.GetService<IBasketRepository>()!, loggerFactory.CreateLogger<BasketService>()));
            _logger.LogInformation("IBasketService registered");
            Status = "Successful register 1/9 database service";
            _locator.RegisterConstant<IOrderPaymentTypeService>( new OrderPaymentTypeService());
            _logger.LogInformation("IOrderPaymentTypeService registered");
            Status = "Successful register 2/9 database service";
            _locator.RegisterConstant<IOrderSaleTypeService>(    new OrderSaleTypeService());
            _logger.LogInformation("IOrderSaleTypeService registered");
            Status = "Successful register 3/9 database service";
            _locator.RegisterConstant<IOrderService>(            new OrderService());
            _logger.LogInformation("IOrderService registered");
            Status = "Successful register 4/9 database service";
            _locator.RegisterConstant<IProductService>(          new ProductService(_locator.GetService<IProductRepository>()!, programLoggerFactory.CreateLogger<ProductService>()));
            _logger.LogInformation("IProductService registered");
            Status = "Successful register 5/9 database service";
            _locator.RegisterConstant<IProductCategoryService>(          new ProductCategoryService(_locator.GetService<IProductCategoryRepository>()!, programLoggerFactory.CreateLogger<ProductCategoryService>()));
            _logger.LogInformation("IProductCategoryService registered");
            Status = "Successful register 6/9 database service";
            _locator.RegisterConstant<IRoleService>(             new RoleService());
            _logger.LogInformation("IRoleService registered");
            Status = "Successful register 7/9 database service";
            _locator.RegisterConstant<IUserService>(             new UserService(_locator.GetService<IUserRepository>()!, programLoggerFactory.CreateLogger<UserService>()));
            _logger.LogInformation("IUserService registered");
            Status = "Successful register 8/9 database service";
            _locator.RegisterConstant<IWarehouseService>(        new WarehouseService());
            _logger.LogInformation("IWarehouseService registered");
            Status = "Successful register 9/9 database service";
            Status = "Successful register all database services";
            _logger.LogInformation("Successful register database services");
            
            // register program services
            _logger.LogInformation("Register program services");
            Status = "Start register program services";  
            _locator.RegisterConstant(new NavigationService(_locator), typeof(INavigationService), "MainNavManager");
            _logger.LogInformation("INavigationService registered");
            Status = "Successful register 1/4 program service";
            _locator.RegisterConstant(new UserStorageService(), typeof(IUserStorageService));
            _logger.LogInformation("IUserStorageService registered");
            Status = "Successful register 2/4 program service";
            _locator.RegisterConstant(new UserBasketService(_locator.GetService<IUserStorageService>()!, _locator.GetService<IBasketService>()!), typeof(IUserBasketService));
            _logger.LogInformation("IUserStorageService registered");
            Status = "Successful register 3/4 program service";
            TopLevel? topLevel = null;
            if (lifeRuntime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                topLevel = TopLevel.GetTopLevel(desktopLifetime.MainWindow);
                _locator.RegisterConstant(new DialogService(desktopLifetime.MainWindow!, loggerFactory.CreateLogger<DialogService>()), typeof(IDialogService));
            }
            else if (lifeRuntime is ISingleViewApplicationLifetime singleViewLifetime)
            {
                topLevel = TopLevel.GetTopLevel(singleViewLifetime.MainView);
                _locator.RegisterConstant(new DialogService((ContentControl)singleViewLifetime.MainView!, loggerFactory.CreateLogger<DialogService>()), typeof(IDialogService));
            }
            if (topLevel == null)
            {
                Status = "The program cannot obtain the storage provider, so the application will stop running, contact your administrator.";
                return;
            }
            _locator.RegisterConstant(new StorageService(topLevel.StorageProvider, loggerFactory.CreateLogger<StorageService>()), typeof(IStorageService));
            _logger.LogInformation("IStorageService registered");
            Status = "Successful register 4/4 program service";
            Status = "Successful register all program services";
            _logger.LogInformation("Successful register program services");
    
            // register factories
            _logger.LogInformation("Register program factories");
            _locator.RegisterConstant(new EditProductViewModelFactory(_locator.GetService<IUserStorageService>()!,_locator.GetService<IProductService>()!, _locator.GetService<IStorageService>()!, _locator.GetService<IDialogService>()!, _locator.GetService<IProductCategoryService>()!), typeof(IEditProductViewModelFactory));
            _logger.LogInformation("IEditProductViewModelFactory registered");
            _logger.LogInformation("Successful register program factories");
            
            // register ViewModels
            Status = "Start register views";
            _locator.Register<SignInViewModel>(           () => new SignInViewModel(               _locator.GetService<IUserStorageService>()!,       _locator.GetService<IUserService>()!));
            _locator.Register<SignUpViewModel>(           () => new SignUpViewModel(                    _locator.GetService<IUserService>()!,         _locator.GetService<IUserStorageService>()!, _locator.GetService<IDialogService>()!));
            _locator.Register<SignOutViewModel>(          () => new SignOutViewModel(                             _locator.GetService<IUserStorageService>()!));
            _locator.Register<HomeViewModel>(             () => new HomeViewModel());
            _locator.Register<CreateProductViewModel>(    () => new CreateProductViewModel(        _locator.GetService<IUserStorageService>()!,    _locator.GetService<IProductService>()!,     _locator.GetService<IStorageService>()!, _locator.GetService<IDialogService>()!, _locator.GetService<IProductCategoryService>()!));
            _locator.Register<ProductCategoriesViewModel>(() => new ProductCategoriesViewModel( _locator.GetService<IProductCategoryService>()!, _locator.GetService<IDialogService>()!));
            
            // register constant ViewModels
            _locator.RegisterConstant<MainViewModel>(    new MainViewModel(    _locator.GetService<IUserStorageService>()!,  _locator.GetService<IDialogService>()!));
            _locator.RegisterConstant<ProductsViewModel>(new ProductsViewModel(_locator.GetService<IUserStorageService>()!, _locator.GetService<IProductService>()!, _locator.GetService<IDialogService>()!, _locator.GetService<IEditProductViewModelFactory>()!, _locator.GetService<IUserBasketService>()!));
    
            // register Views
            _locator.Register(() => new MainView(),              typeof(IViewFor<MainViewModel>));
            _locator.Register(() => new HomeView(),              typeof(IViewFor<HomeViewModel>));
            _locator.Register(() => new SignInView(),            typeof(IViewFor<SignInViewModel>));
            _locator.Register(() => new SignUpView(),            typeof(IViewFor<SignUpViewModel>));
            _locator.Register(() => new SignOutView(),           typeof(IViewFor<SignOutViewModel>));
            _locator.Register(() => new ProductsView(),          typeof(IViewFor<ProductsViewModel>));
            _locator.Register(() => new CreateProductView(),     typeof(IViewFor<CreateProductViewModel>));
            _locator.Register(() => new ProductCategoriesView(), typeof(IViewFor<ProductCategoriesViewModel>));
            Status = "Successful register all views";
    
            Status = "Starting...";
            _logger.LogInformation("Start App");
        
        }
        catch (Exception e)
        {
            Status = "Error with initialize app, talk about administrator";
            _logger?.LogError("error while app is starting.\nMessage\t{Message}.\nInnerException\t{InnerException}", e.Message, e.InnerException);
            return;
        }
        
        NavigationService = _locator.GetService<INavigationService>("MainNavManager")!;
        
        await Task.Run(LoadMainView);
    }

    private async void LoadMainView()
    {
        await Dispatcher.UIThread.InvokeAsync(new Action(RemoveLoadPanelMethod));
        await NavigationService!.NavigateTo<MainViewModel>();
    }

    private async void RemoveLoadPanelMethod()
    {
        await RemoveLoadPanel.Handle(Unit.Default);
    }
}