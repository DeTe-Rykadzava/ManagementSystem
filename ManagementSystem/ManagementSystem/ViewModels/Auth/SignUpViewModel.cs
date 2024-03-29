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

namespace ManagementSystem.ViewModels.Auth;

public class SignUpViewModel : RoutableViewModelBase
{
    // services
    private readonly IUserService _userService;
    private readonly IUserStorageService _userStorageService;
    
    public override string ViewModelViewPath { get; } = "signUp";
    public override INavigationService RootNavManager { get; protected set; } = null!;

    // fields
    private string _status = string.Empty;
    public string Status
    {
        get => _status;
        private set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    // create model
    public UserCreateViewModel UserCreateModel { get; }
    
    // commands
    public ICommand SignUpCommand { get; }
    public ICommand GoToBackCommand { get; }

    public SignUpViewModel(IUserService userService, IUserStorageService userStorageService)
    {
        _userService = userService;
        _userStorageService = userStorageService;
        UserCreateModel = new UserCreateViewModel();
        var canSignUp = this.WhenAnyValue(x => x.UserCreateModel.IsValid).DistinctUntilChanged();
        SignUpCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Status = "";
            var createResult = await _userService.CreateUser(UserCreateModel);
            if (createResult == false)
            {
                Status = "Cannot create user, check value, may be user with current login already exist";
                var box = MessageBoxManager.GetMessageBoxStandard("SignUpErr", "sorry, but the user was not registered",
                    ButtonEnum.Ok, Icon.Error, WindowStartupLocation.CenterOwner);
                var result = await box.ShowAsync();
                return;
            }

            var user = await _userService.GetUserByLoginPassword(UserCreateModel.Login, UserCreateModel.Password);
            if(user == null)
                return;
            _userStorageService.CurrentUser = user;
            await RootNavManager.NavigateTo<MainViewModel>();
        }, canSignUp);
        GoToBackCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
    }
}