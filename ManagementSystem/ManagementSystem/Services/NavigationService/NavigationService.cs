using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using Splat;

namespace ManagementSystem.Services.NavigationService;

public sealed class NavigationService : INavigationService
{
    private RoutableViewModelBase? _currentViewModel;
    public RoutableViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        private set => Set(ref _currentViewModel, value);
    }

    private int _currentIndex = 0;
    
    private List<RoutableViewModelBase> _history = new List<RoutableViewModelBase>();

    public async Task GoBack()
    {
        _currentIndex -= 1;
        if (_currentIndex < 0)
            _currentIndex = 0;
        CurrentViewModel = _history[_currentIndex];
        if (CurrentViewModel != null)
            await CurrentViewModel.OnShowed();
    }

    public async Task NavigateTo<T>()
    {
        var vm = Locator.GetLocator().GetService<T>();
        if(vm == null) return;
        if (vm is not RoutableViewModelBase viewModel) return;
        var historyVm = _history.FirstOrDefault(x => x == viewModel);
        if (historyVm == null)
        {
            _history.Add(viewModel);
            historyVm = viewModel;
            _currentIndex = _history.Count - 1;
        }
        CurrentViewModel = historyVm;
        _currentIndex = _history.IndexOf(historyVm);
        
        CurrentViewModel.OnInitialized(this); 
        await CurrentViewModel.OnShowed();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}