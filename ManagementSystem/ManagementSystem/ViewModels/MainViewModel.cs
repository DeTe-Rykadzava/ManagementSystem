using System;
using ReactiveUI;
using Splat;

namespace ManagementSystem.ViewModels;

public class MainViewModel : ViewModelBase, IRoutableViewModel
{
    public string Test { get; set; } = string.Empty;
    public string? UrlPathSegment { get; } = "main";
    public IScreen HostScreen { get; }

    public MainViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }
}