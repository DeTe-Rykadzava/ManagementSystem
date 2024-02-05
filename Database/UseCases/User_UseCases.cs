using Database.Data;
using Database.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace Database.UseCases;

public class User_UseCases
{
    public static async Task<UserModel?> GetUserById(ManagementSystemDatabaseContext context,int id)
    {
        try
        {
            var user = await context.Users
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
    
    public static async Task<UserModel?> GetUserByLoginPassword(ManagementSystemDatabaseContext context, string login, string password)
    {
        try
        {
            var user = await context.Users
                .AsQueryable()
                .AsNoTracking()
                .Include(i => i.UserInfo)
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

    public static async Task<bool> CreateUser(ManagementSystemDatabaseContext context, UserCreateModel model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
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
                HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            await context.UserInfos.AddAsync(userInfo);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}