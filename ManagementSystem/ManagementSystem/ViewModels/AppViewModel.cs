using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;

namespace ManagementSystem.ViewModels;

public class AppViewModel : ViewModelBase, IScreen
{
    public string? UrlPathSegment { get; } = "app";
    public RoutingState Router { get; }
    
    public ReactiveCommand<Unit, IRoutableViewModel?> GoNext { get; }

    // The command that navigates a user back.
    public ReactiveCommand<Unit, Unit> GoBack { get; }
    public AppViewModel()
    {
        Router = new RoutingState();
        
        Locator.GetLocator().Register<MainViewModel>(() => new MainViewModel(this));
        
        // Locator.CurrentMutable.Register(() => new MainViewModel(this), typeof(IViewFor<MainViewModel>));
        
        GoNext = ReactiveCommand.CreateFromObservable(() =>
            {
                try
                {
                    var vm = Locator.GetLocator().GetService<IViewFor<MainViewModel>>();
                    if (vm == null || vm.ViewModel == null) return null;
                    return Router.Navigate.Execute(vm.ViewModel);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return null;
            }
            );
        // You can also ask the router to go back. One option is to 
        // execute the default Router.NavigateBack command. Another
        // option is to define your own command with custom
        // canExecute condition as such:
        var canGoBack = this
            .WhenAnyValue(x => x.Router.NavigationStack.Count)
            .Select(count => count > 0);
        GoBack = ReactiveCommand.Create(
            () =>
            {
                Router.NavigateBack.Execute(Unit.Default);
                return Unit.Default;
            },
            canGoBack);
    }

}