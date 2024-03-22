using System.Reactive;
using System.Threading.Tasks;
using ManagementSystem.Services;

namespace ManagementSystem.ViewModels.Core;

public abstract class RoutableViewModelBase : ViewModelBase
{
    public abstract NavigationService RootNavManager { get; protected set; }

    public virtual async Task OnInitialized(NavigationService rootNavManager)
    {
        RootNavManager = rootNavManager;
    }

    public virtual Task OnShowed() { return Task.FromResult(Unit.Default); }
}