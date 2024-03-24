using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.User;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class UserService : IUserService
{
    public Task<UserViewModel?> GetUserById(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<UserViewModel?> GetUserByLoginPassword(string login, string password)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> CreateUser(UserCreateViewModel model)
    {
        throw new System.NotImplementedException();
    }
}