using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.User;
using ManagementSystem.ViewModels.Main;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using IDialogService = ManagementSystem.Services.DialogService.IDialogService;

namespace ManagementSystem.ViewModels.Auth;

public class SignUpViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "signUp";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IUserService _userService;
    private readonly IUserStorageService _userStorageService;
    private readonly IDialogService _dialogService;
    
    // fields
    private string? _status = null;
    public string? Status
    {
        get => _status;
        private set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    // create model
    public UserCreateViewModel UserCreateModel { get; }
    
    // commands
    public ICommand SignUpCommand { get; }
    public ICommand GoToBackCommand { get; }

    public SignUpViewModel(IUserService userService, IUserStorageService userStorageService, IDialogService dialogService)
    {
        _userService = userService;
        _userStorageService = userStorageService;
        _dialogService = dialogService;
        UserCreateModel = new UserCreateViewModel();
        var canSignUp = this.WhenAnyValue(x => x.UserCreateModel.IsValid).DistinctUntilChanged();
        SignUpCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Status = null;
            var createResult = await _userService.CreateUser(UserCreateModel);
            if (!createResult.IsSuccess || createResult.Value == null)
            {
                Status = $"Cannot create user, purpose:\n\t* {string.Join("\n\t* ", createResult.Statuses)}";
                await _dialogService.ShowPopupDialogAsync("SignUpErr", $"Sorry, but the user was not registered, purpose:\n\t* {string.Join("\n\t* ", createResult.Statuses)}", icon: Icon.Error);
                return;
            }

            _userStorageService.CurrentUser = createResult.Value;
            await RootNavManager.NavigateTo<MainViewModel>();
        }, canSignUp);
        GoToBackCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
    }
}