using System;
using System.Collections.Generic;
using System.Reactive;
using System.ComponentModel.DataAnnotations;
using Database.Models.UserModels;
using ManagementSystem.Assets;
using ManagementSystem.ViewModels.Core;
using ReactiveUI;

namespace ManagementSystem.ViewModels.DataVM.User;

public class UserCreateViewModel : ViewModelBase
{
    private string _login = string.Empty;
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    private string _password = string.Empty;
    
    [Required]
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private string _firstName = string.Empty;
    
    [Required]
    public string FirstName
    {
        get => _firstName;
        set => this.RaiseAndSetIfChanged(ref _firstName, value);
    }

    private string _lastName = string.Empty;
    
    [Required]
    public string LastName
    {
        get => _lastName;
        set => this.RaiseAndSetIfChanged(ref _lastName, value);
    }

    private string? _patronymic = string.Empty;
    
    public string? Patronymic
    {
        get => _patronymic;
        set => this.RaiseAndSetIfChanged(ref _patronymic, value);
    }

    private bool _isValid = default;
    public bool IsValid
    {
        get => _isValid;
        set => this.RaiseAndSetIfChanged(ref _isValid, value);
    }

    public UserCreateViewModel()
    {
        this.WhenAnyValue(
                x => x.Login,
                x => x.Password,
                x => x.FirstName,
                x => x.LastName)
            .Subscribe(param =>
            {
                var context = new ValidationContext(this);
                var results = new List<ValidationResult>();

                IsValid = Validator.TryValidateObject(this, context, results, true);
            });
    }
    
    public UserCreateModel ToBaseModel()
    {
        return new UserCreateModel
        {
            Login = Login,
            Password = Password,
            FirstName = FirstName,
            LastName = LastName,
            Patronymic = Patronymic,
            RoleId = StaticResources.ClientRoleId
        };
    }
}