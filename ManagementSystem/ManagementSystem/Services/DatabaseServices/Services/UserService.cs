using System;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models.Core;
using DynamicData;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.User;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<IUserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger) =>
        (_userRepository, _logger) = (userRepository, logger);
    
    public async Task<ActionResultViewModel<UserViewModel>> GetUserById(int id)
    {
        var result = new ActionResultViewModel<UserViewModel>();
        try
        {
            var userResult = await _userRepository.GetUserById(id);
            if (!userResult.Success || userResult.Value == null)
            {
                result.Statuses.Add("Failed get data");
                result.Statuses.Add("User not found");
            }
            else
            {
                result.IsSuccess = true;
                result.Value = new UserViewModel(userResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user by Id: {Id}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            result.Statuses.Add("Failed get data");
            result.Statuses.Add("Application Error");
        }

        return result;
    }

    public async Task<ActionResultViewModel<UserViewModel>> GetUserByLoginPassword(string login, string password)
    {
        var result = new ActionResultViewModel<UserViewModel>();
        try
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                result.Statuses.Add("Failed get data");
                result.Statuses.Add("Input data not valid");
            }
            else
            {
                var userResult = await _userRepository.GetUserByLoginPassword(login, password);
                if (!userResult.Success || userResult.Value == null)
                {
                    result.Statuses.Add("Failed get data");
                    result.Statuses.Add("User not found");
                }
                else
                {
                    result.IsSuccess = true;
                    result.Value = new UserViewModel(userResult.Value);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user by login and password.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed get data");
            result.Statuses.Add("Application error");
        }
        return result;
    }

    public async Task<ActionResultViewModel<UserViewModel>> CreateUser(UserCreateViewModel model)
    {
        var result = new ActionResultViewModel<UserViewModel>();
        try
        {
            var userResult = await _userRepository.CreateUser(model.ToBaseModel());
            if (!userResult.Success || userResult.Value == null)
            {
                result.Statuses.Add("Failed add data");
                result.Statuses.Add("User not created");
            }
            else
            {
                result.IsSuccess = true;
                result.Value = new UserViewModel(userResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Can not save user in base.\nException:\t{Message}.\nInner Exception:\t{InnerException}",e.Message, e.InnerException);
            result.Statuses.Add("Failed get data");
            result.Statuses.Add("Application error");
        }

        return result;
    }
}