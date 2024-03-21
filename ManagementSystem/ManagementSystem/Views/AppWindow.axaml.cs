using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Views;

public partial class AppWindow : ReactiveWindow<AppViewModel>
{
    public AppWindow()
    {
        InitializeComponent();
    }
}