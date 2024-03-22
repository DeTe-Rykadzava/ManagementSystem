using Avalonia.ReactiveUI;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}