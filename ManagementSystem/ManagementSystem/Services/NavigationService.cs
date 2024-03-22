using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ManagementSystem.ViewModels;
using ManagementSystem.ViewModels.Core;
using ReactiveUI;
using Splat;

namespace ManagementSystem.Services;

public sealed class NavigationService : INotifyPropertyChanged
{
    private ViewModelBase? _currentViewModel;
    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set => Set(ref _currentViewModel, value);
    }

    private int _currentIndex = 0;
    
    private List<ViewModelBase> _history = new List<ViewModelBase>();

    public async Task GoBack()
    {
        _currentIndex -= 1;
        if (_currentIndex < 0)
            _currentIndex = 0;
        CurrentViewModel = _history[_currentIndex];
        if (CurrentViewModel is RoutableViewModelBase routableVM)
            await routableVM.OnShowed();
    }

    public async Task NavigateTo<T>()
    {
        var vm = Locator.GetLocator().GetService<T>();
        if(vm == null) return;
        if (vm is not ViewModelBase viewModel) return;
        var historyVm = _history.FirstOrDefault(x => x == viewModel);
        if (historyVm == null)
        {
            _history.Add(viewModel);
            historyVm = viewModel;
            _currentIndex = _history.Count - 1;
        }
        CurrentViewModel = historyVm;
        _currentIndex = _history.IndexOf(historyVm);
        if (CurrentViewModel is RoutableViewModelBase routVm)
        {
            await routVm.OnInitialized(this);
            await routVm.OnShowed();
        }

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