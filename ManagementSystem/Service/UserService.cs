using Database.Data;
using Database.Models.UserModels;
using Database.UseCases;

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
            _logger.LogError($"Error with get user by id.\n{e.Message}\n{e.InnerException}");
            return null;
        }
    }

    public async Task<UserModel?> GetUserByLoginPassword()
    {
        
    }

}