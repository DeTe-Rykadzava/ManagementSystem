using System;
using ManagementSystem.ViewModels;
using ManagementSystem.Views;
using ReactiveUI;

namespace ManagementSystem;

public class AppViewLocator :  IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        MainViewModel context => new MainView() { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}