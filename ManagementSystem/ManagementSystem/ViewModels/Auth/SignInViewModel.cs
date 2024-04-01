using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Windows.Input;
using Database.Interfaces;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.Main;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Auth;

public class SignInViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "signIn";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IUserStorageService _userStorageService;
    private readonly IUserService _userService;
    
    // commands
    public ICommand GoToBackCommand { get; }
    public ICommand SignInCommand { get; }
    
    // fields
    private string? _status = null;
    public string? Status
    {
        get => _status;
        private set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    private string _login = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "The login is required")]
    [EmailAddress(ErrorMessage = "The login must be an email address")]
    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }
    
    private string _password = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "The password is required")]
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    
    public SignInViewModel(IUserStorageService userStorageService, IUserService userService)
    {
        _userStorageService = userStorageService;
        _userService = userService;
        GoToBackCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
        var canSignIn = this.WhenAnyValue(
            x => x.Login, x => x.Password,
            (login, password) =>
                !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password))
            .DistinctUntilChanged();
        SignInCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            try
            {
                Status = null;
                var userResult = await _userService.GetUserByLoginPassword(Login, Password);
                if (!userResult.IsSuccess || userResult.Value == null)
                {
                    Status = $"User not founded, purpose:\n\t* {string.Join("\n\t* ", userResult.Statuses)}";
                    return;
                }
                _userStorageService.CurrentUser = userResult.Value;
                await RootNavManager!.NavigateTo<MainViewModel>();
            }
            catch (Exception)
            {
                return;
            } 
        }, canSignIn);
    }
}