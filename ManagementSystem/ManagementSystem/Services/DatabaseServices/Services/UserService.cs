using System;
using System.Threading.Tasks;
using Database.Interfaces;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.User;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<IUserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<IUserService> logger) =>
        (_userRepository, _logger) = (userRepository, logger);
    
    public async Task<UserViewModel?> GetUserById(int id)
    {
        try
        {
            var user = await _userRepository.GetUserById(id);
            return user == null ? null : new UserViewModel(user);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user by Id: {Id}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<UserViewModel?> GetUserByLoginPassword(string login, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return null;
            var user = await _userRepository.GetUserByLoginPassword(login, password);
            return user == null ? null : new UserViewModel(user);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user by login and password.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<bool> CreateUser(UserCreateViewModel model)
    {
        try
        {
            var result = await _userRepository.CreateUser(model.ToBaseModel());
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Can not save user in base.\nException:\t{Message}.\nInner Exception:\t{InnerException}",e.Message, e.InnerException);
            return false;
        }
    }
}