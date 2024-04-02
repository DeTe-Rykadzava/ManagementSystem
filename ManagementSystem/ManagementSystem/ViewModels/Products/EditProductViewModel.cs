using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.Products;

public class EditProductViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "edit_product";
    public override INavigationService RootNavManager { get; protected set; } = null!;
}