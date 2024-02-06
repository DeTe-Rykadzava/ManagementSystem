using Database.Data;
using Database.Models.UserModels;
using Database.UseCases;
using ManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ManagementSystem.Service;

public class UserService
{
    private readonly ManagementSystemDatabaseContext _context;

    private readonly ILogger<UserService> _logger;
    
    public UserService(ManagementSystemDatabaseContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserModel?> GetUserById(int id)
    {
        try
        {
            var user = await User_UseCases.GetUserById(_context,id);
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
            var user = await User_UseCases.GetUserByLoginPassword(_context, signInModel.Login, signInModel.Password);
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
            return await User_UseCases.CreateUser(_context, model.ConvertToDatabaseModel());
        }
        catch (Exception e)
        {
            _logger.LogError("Error with create user.\n{Message}\n{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

}