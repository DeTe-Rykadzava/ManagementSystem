using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using ManagementSystem.Assets;
using ManagementSystem.ViewModels;

namespace ManagementSystem.Views;

public partial class AppWindow : ReactiveWindow<AppViewModel>
{
    public AppWindow()
    {
        InitializeComponent();
        Title = StaticResources.AppName;
        Icon = new WindowIcon(AssetLoader.Open(new Uri(StaticResources.AppIconResourceLink)));
    }
}