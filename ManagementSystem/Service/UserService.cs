using Database.Data;
using Database.Interfaces;
using Database.Models.UserModels;
using ManagementSystem.AuthModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ManagementSystem.Service;

public class UserService
{
    private readonly ILogger<UserService> _logger;

    private readonly IUserRepository _userRepository;
    
    public UserService(ILogger<UserService> logger, IUserRepository repository)
    {
        _logger = logger;
        _userRepository = repository;
    }

    public async Task<UserModel?> GetUserById(int id)
    {
        try
        {
            var user = await _userRepository.GetUserById(id);
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user by id.\n{Messagee}\n{InnerExceptionon}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<UserModel?> GetUserByModel(SignInModel signInModel)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(signInModel.Login) || string.IsNullOrWhiteSpace(signInModel.Password))
                return null;
            var user = await _userRepository.GetUserByLoginPassword(signInModel.Login, signInModel.Password);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError("Error with get user.\n{Message}\n{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<bool> SignUpUser(SignUpModel model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
                return false;
            return await _userRepository.CreateUser(model.ConvertToDatabaseModel(), _logger);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with create user.\n{Message}\n{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

}