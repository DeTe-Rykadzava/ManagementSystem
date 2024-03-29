using System.Windows.Input;
using ManagementSystem.Services;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.Main;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Auth;

public class SignOutViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "signOut";
    public override INavigationService RootNavManager { get; protected set; } = null!;

    // services
    private readonly IUserStorageService _userStorageService;
    
    // commnads
    public ICommand SignOutCommand { get; }
    public ICommand GoToBackCommand { get; }

    public SignOutViewModel(IUserStorageService userStorageService)
    {
        _userStorageService = userStorageService;
        
        SignOutCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            _userStorageService.CurrentUser = null;
            await RootNavManager.NavigateTo<MainViewModel>();
        });
        GoToBackCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
    }
}