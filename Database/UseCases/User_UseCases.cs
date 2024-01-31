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
}