using Database.Data;
using Database.Interfaces;
using Database.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    public UserRepository(IManagementSystemDatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<UserModel?> GetUserById(int id)
    {
        try
        {
            var user = await _context.Users
                .AsQueryable()
                .AsNoTracking()
                .Include(i => i.UserInfo)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return null;
            return new UserModel(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public async Task<UserModel?> GetUserByLoginPassword(string login, string password)
    {
        try
        {
            var user = await _context.Users
                .AsQueryable()
                .AsNoTracking()
                .Include(i => i.UserInfo)
                .ThenInclude(i => i.Role)
                .FirstOrDefaultAsync(x => x.Login == login);
            if (user == null)
                return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
                return null;
            return new UserModel(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<bool> CreateUser(UserCreateModel model, ILogger? logger = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
                return false;

            var existedUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == model.Login);
            if (existedUser != null)
                return false;
            
            var userInfo = new UserInfo
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                RoleId = model.RoleId
            };
            
            var user = new User
            {
                Login = model.Login,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password),
            };

            await _context.UserInfos.AddAsync(userInfo);
            await _context.SaveChangesAsync();
            user.UserInfoId = userInfo.Id;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            logger?.LogError("Can not save user in base.\nException:\t{Message}.\nInner Exception:\t{InnerException}",e.Message, e.InnerException);
            return false;
        }
    }
}