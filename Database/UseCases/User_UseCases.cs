using Database.Data;
using Database.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace Database.UseCases;

public static class User_UseCases
{
    private static User? _currentUser;
    
    public static User? GetCurrentUser()
    {
        return _currentUser;
    }

    public static async Task<UserModel?> CreateNewUserInfo(IDataDatabaseContext context, NewUserInfoModel newUser)
    {
        try
        {
            var role = newUser.Role == null ? newUser.Role : await Role_UseCases.GetRoleById(context, 2);
            if (role == null)
            {
                throw new Exception("Can not create a user, role problem");
            }

            var userInfo = new UserInfo
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Patronymic = newUser.Patronymic,
                RoleId =  role.Id
            };

            await context.UserInfos.AddAsync(userInfo);
            // await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var userDb = await context.UserInfos.AsQueryable().AsNoTracking()
                .Include(i => i.Role)
                .FirstOrDefaultAsync(x => x.Id == newUser.UserId);
            return new UserInfoModel(userDb!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public static async Task<UserInfoModel?> GetUserInfoByUserId(IDataDatabaseContext context, int id)
    {
        try
        {
            var userInfo = await context.UserInfos.AsQueryable().AsNoTracking()
                .Include(i => i.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (userInfo == null)
                return null;

            return new UserInfoModel(userInfo);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}