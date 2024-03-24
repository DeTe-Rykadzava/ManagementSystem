using ManagementSystem.Services;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Main;

public class HomeViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "home";
    public override INavigationService RootNavManager { get; protected set; }
}