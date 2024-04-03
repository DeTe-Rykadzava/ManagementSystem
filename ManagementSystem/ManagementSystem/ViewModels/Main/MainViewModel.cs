using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ManagementSystem.Services;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Auth;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.User;
using ManagementSystem.ViewModels.Products;
using ReactiveUI;
using Splat;

namespace ManagementSystem.ViewModels.Main;

public class MainViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "main";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    public INavigationService SubNavigationService { get; }
    public IUserStorageService UserStorageService { get; }

    // Sign Commands
    public ICommand GoToSignInCommand { get; }
    public ICommand GoToSignUpCommand { get; }
    public ICommand GoToSignOutCommand { get; }
    
    // Navigation Commands
    public ICommand GoToHomeCommand { get; }
    public ICommand GoToProductsCommand { get; }
    public ICommand GoToWarehousesCommand { get; }
    public ICommand GoToUserBasketCommand { get; }
    public ICommand GoToUserOrdersCommand { get; }
    
    public MainViewModel(IUserStorageService userStorageService)
    {
        UserStorageService = userStorageService;
        Locator.GetLocator().RegisterConstant(new NavigationService(Locator.GetLocator()), typeof(INavigationService), "SubNavManager");
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
        GoToWarehousesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SubNavigationService.NavigateTo<ProductsViewModel>();
        });
        GoToUserBasketCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SubNavigationService.NavigateTo<ProductsViewModel>();
        });
        GoToUserOrdersCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SubNavigationService.NavigateTo<ProductsViewModel>();
        });

        GoToHomeCommand.Execute(null);
    }
}