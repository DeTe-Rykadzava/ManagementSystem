using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Product;

public class ProductsViewModel : RoutableViewModelBase
{
    public override INavigationService RootNavManager { get; protected set; } = null!;
    public override string ViewModelViewPath { get; } = "products";
}