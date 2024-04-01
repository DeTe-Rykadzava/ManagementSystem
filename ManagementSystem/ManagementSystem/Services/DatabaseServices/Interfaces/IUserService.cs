using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.User;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IUserService
{
    public Task<ActionResultViewModel<UserViewModel>> GetUserById (int id);

    public Task<ActionResultViewModel<UserViewModel>> GetUserByLoginPassword(string login, string password);

    public Task<ActionResultViewModel<UserViewModel>> CreateUser(UserCreateViewModel model);
}