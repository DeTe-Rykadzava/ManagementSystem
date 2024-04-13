using System;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models.UserModels;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DatabaseServices.Services;
using ManagementSystem.ViewModels.Core;
using Splat;

namespace ManagementSystem.ViewModels.DataVM.User;

public class UserViewModel : ViewModelBase
{
    private readonly UserModel _user;

    public int Id => _user.Id;
    
    public int UserInfoId => _user.UserInfoId;
    
    public string Login => _user.Login;

    public string FirstName => _user.FirstName;

    public string LastName=> _user.LastName;

    public string? Patronymic => _user.Patronymic;
    
    public int RoleId => _user.RoleId;

    public string Role => _user.Role.RoleName;

    public string FullName => 
        $"{(string.IsNullOrWhiteSpace(LastName) ? "" : $"{LastName} ")}" +
        $"{(string.IsNullOrWhiteSpace(FirstName)? "" : $"{FirstName} ")}" +
        $"{(string.IsNullOrWhiteSpace(Patronymic)? "" : $"{Patronymic} ")}";
    
    public UserViewModel(UserModel model)
    {
        _user = model;
    }
}