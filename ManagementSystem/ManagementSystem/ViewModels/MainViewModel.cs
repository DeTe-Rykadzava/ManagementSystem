using System;
using System.Threading.Tasks;
using ManagementSystem.Assets;
using ManagementSystem.Services;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.User;
using ReactiveUI;
using Splat;

namespace ManagementSystem.ViewModels;

public class MainViewModel : RoutableViewModelBase
{
    public string Test { get; set; } = string.Empty;
    public override NavigationService? RootNavManager { get; protected set; } = null;

    private UserViewModel? _currentUser = null;
    public UserViewModel? CurrentUser
    {
        get => _currentUser;
        private set => this.RaiseAndSetIfChanged(ref _currentUser, value);
    }

    public NavigationService SubNavigationService { get; }
    
    public MainViewModel()
    {
        Locator.GetLocator().RegisterConstant(new NavigationService(), "SubNavManager");
        SubNavigationService = Locator.GetLocator().GetService<NavigationService>("SubNavManager")!;
    }
    
    public override async Task OnShowed()
    {
        if (StaticResources.CurrentUser != null && CurrentUser == null)
            CurrentUser = StaticResources.CurrentUser;
    }
}