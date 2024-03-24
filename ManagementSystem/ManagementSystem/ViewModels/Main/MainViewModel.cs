using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ManagementSystem.Services;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Auth;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.User;
using ReactiveUI;
using Splat;

namespace ManagementSystem.ViewModels.Main;

public class MainViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "main";
    public override INavigationService? RootNavManager { get; protected set; } = null;
    
    // services
    public INavigationService SubNavigationService { get; }
    public IUserStorageService UserStorageService { get; }

    // Sign Commands
    public ICommand GoToSignInCommand { get; }
    public ICommand GoToSignUpCommand { get; }
    public ICommand GoToSignOutCommand { get; }
    
    // Navigation Commands
    public ICommand GoToHomeCommand { get; }
    
    public MainViewModel(IUserStorageService userStorageService)
    {
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
        
        UserStorageService = userStorageService;
        Locator.GetLocator().RegisterConstant(new NavigationService(), typeof(INavigationService), "SubNavManager");
        SubNavigationService = Locator.GetLocator().GetService<INavigationService>("SubNavManager")!;

        GoToHomeCommand.Execute(null);
    }
}