using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Order;

public class CreateOrderViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "create_order";
    public override INavigationService RootNavManager { get; protected set; } = null!;
}