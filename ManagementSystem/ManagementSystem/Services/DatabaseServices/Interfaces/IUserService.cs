using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.User;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IUserService
{
    public Task<UserViewModel?> GetUserById (int id);

    public Task<UserViewModel?> GetUserByLoginPassword(string login, string password);

    public Task<bool> CreateUser(UserCreateViewModel model);
}