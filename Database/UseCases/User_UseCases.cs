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

    public static async Task<UserModel?> CreateNewUser(NewUserModel newUser)
    {
        try
        {
            var user = new User
            {
                Login = newUser.Login,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
            };

            var role = newUser.Role == null ? newUser.Role : await Role_UseCases.GetRoleById(2);
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

            await ManagementSystemDatabaseContext.Context.UserInfos.AddAsync(userInfo);
            await ManagementSystemDatabaseContext.Context.Users.AddAsync(user);
            await ManagementSystemDatabaseContext.Context.SaveChangesAsync();

            var userDb = await ManagementSystemDatabaseContext.Context.Users.AsQueryable().AsNoTracking()
                .Include(i => i.UserInfo)
                .ThenInclude(i => i.Role)
                .FirstOrDefaultAsync(x => x.Id == user.Id);
            return new UserModel(userDb!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public static async Task<UserModel?> SignInUser(string login, string password)
    {
        try
        {
            var user = await ManagementSystemDatabaseContext.Context.Users.AsQueryable().AsNoTracking()
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

    public static async Task<UserModel?> GetUserById(int id)
    {
        try
        {
            var user = await ManagementSystemDatabaseContext.Context.Users.AsQueryable().AsNoTracking()
                .Include(i => i.UserInfo)
                .ThenInclude(i => i.Role)
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
}