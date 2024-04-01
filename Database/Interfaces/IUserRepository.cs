using Database.Models.Core;
using Database.Models.UserModels;

namespace Database.Interfaces;

public interface IUserRepository
{
    public Task<ActionResultModel<UserModel>> GetUserById (int id);

    public Task<ActionResultModel<UserModel>> GetUserByLoginPassword(string login, string password);

    public Task<ActionResultModel<UserModel>> CreateUser(UserCreateModel model);
}