using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ManagementSystem.Assets;
using ManagementSystem.ViewModels;
using ManagementSystem.Views;

namespace ManagementSystem;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new AppWindow()
            {
                Title = StaticResources.AppName,
                DataContext = new AppViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new AppView()
            {
                DataContext = new AppViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}