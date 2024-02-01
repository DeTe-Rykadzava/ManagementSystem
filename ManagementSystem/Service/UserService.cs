using Database.Data;
using Database.Models.UserModels;
using Database.UseCases;
using ManagementSystem.Models;

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

    public async Task<UserModel?> GetUserByModel(LoginModel loginModel)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(loginModel.Login) || string.IsNullOrWhiteSpace(loginModel.Password))
                return null;
            var user = await User_UseCases.GetUserByLoginPassword(_context, loginModel.Login, loginModel.Password);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError("Error with get user.\n{Message}\n{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

}