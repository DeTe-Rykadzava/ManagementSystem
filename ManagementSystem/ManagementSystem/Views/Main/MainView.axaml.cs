using Avalonia.ReactiveUI;
using ManagementSystem.ViewModels;
using ManagementSystem.ViewModels.Main;

namespace ManagementSystem.Views.Main;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}