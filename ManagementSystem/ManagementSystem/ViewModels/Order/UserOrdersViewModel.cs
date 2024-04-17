using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Order;

public class UserOrdersViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "user_orders";
    public override INavigationService RootNavManager { get; protected set; } = null!;
}