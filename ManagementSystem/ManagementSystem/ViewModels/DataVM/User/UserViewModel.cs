using System;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models.UserModels;
using ManagementSystem.ViewModels.Core;
using Splat;

namespace ManagementSystem.ViewModels.DataVM.User;

public class UserViewModel : ViewModelBase
{
    private readonly UserModel _user;

    public int UserInfoId => _user.UserInfoId;
    
    public string Login => _user.Login;

    public string FirstName => _user.FirstName;

    public string LastName=> _user.LastName;

    public string? Patronymic => _user.Patronymic;
    
    public int RoleId => _user.RoleId;

    public string Role => _user.Role.RoleName;

    public string FullName => $"{LastName} {FirstName} {Patronymic}";
    
    public UserViewModel(UserModel model)
    {
        _user = model;
    }

    public static async Task<UserViewModel?> GetUserById(int id)
    {
        try
        {
            var userRepository = Locator.GetLocator().GetService<IUserRepository>();
            if (userRepository == null) return null;

            var model = await userRepository.GetUserById(id);

            if (model == null) return null;

            return new UserViewModel(model);
        }
        catch (Exception e)
        {
            
            return null;
        }
    }
    
    public static async Task<UserViewModel?> GetUserByLoginPassword(string login, string password)
    {
        try
        {
            var userRepository = Locator.GetLocator().GetService<IUserRepository>();
            if (userRepository == null) return null;

            var model = await userRepository.GetUserByLoginPassword(login, password);

            if (model == null) return null;

            return new UserViewModel(model);
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    public static async Task<bool> CreateUser(UserCreateViewModel model)
    {
        try
        {
            var userRepository = Locator.GetLocator().GetService<IUserRepository>();
            if (userRepository == null) return false;

            var user = await userRepository.CreateUser(model.ToBaseModel());

            if (user == false) return false;

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
}