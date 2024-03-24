using System.Reactive;
using System.Threading.Tasks;
using ManagementSystem.Services;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Auth;

public class SignUpViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "signUp";
    public override INavigationService RootNavManager { get; protected set; }
}