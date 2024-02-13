using Database.Models.UserModels;
using Microsoft.Extensions.Logging;

namespace Database.Interfaces;

public interface IUserRepository
{
    public Task<UserModel?> GetUserById (int id);

    public Task<UserModel?> GetUserByLoginPassword(string login, string password);

    public Task<bool> CreateUser(UserCreateModel model, ILogger? logger = null);
}