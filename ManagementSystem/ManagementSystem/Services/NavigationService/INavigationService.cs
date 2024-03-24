using System.ComponentModel;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.Services.NavigationService;

public interface INavigationService : INotifyPropertyChanged
{
    public RoutableViewModelBase? CurrentViewModel { get; }
    public Task GoBack();
    public Task NavigateTo<T>();
}