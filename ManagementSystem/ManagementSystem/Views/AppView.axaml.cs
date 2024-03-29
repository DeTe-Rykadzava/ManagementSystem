using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ManagementSystem.ViewModels;
using ReactiveUI;

namespace ManagementSystem.Views;

public partial class AppView : ReactiveUserControl<AppViewModel>
{
    public AppView()
    {
        this.WhenActivated(d => d(ViewModel!.RemoveLoadPanel.RegisterHandler(Handler)));
        InitializeComponent();
    }

    private void Handler(IInteractionContext<Unit, Unit> obj)
    {
        var rootPanel = this.GetControl<Grid>("ContentGrid");
        var panel = this.GetControl<Panel>("LoadPanel");
        rootPanel.Children.Remove(panel);
        obj.SetOutput(Unit.Default);
    }
}