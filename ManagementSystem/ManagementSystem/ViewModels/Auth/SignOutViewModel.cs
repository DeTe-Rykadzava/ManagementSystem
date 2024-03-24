using ManagementSystem.Services;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Auth;

public class SignOutViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "signOut";
    public override INavigationService RootNavManager { get; protected set; }
}