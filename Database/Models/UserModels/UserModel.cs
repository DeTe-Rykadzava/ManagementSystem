using Database.Data;

namespace Database.Models.UserModels;

public class UserModel
{
    private readonly User _user;

    public int Id => _user.Id;

    public string FirstName => _user.UserInfo.FirstName;

    public string LastName => _user.UserInfo.LastName;

    public string? Patronymic => _user.UserInfo.Patronymic;

    public string RoleName => _user.UserInfo.Role.RoleName;
    
    public UserModel(User user)
    {
        _user = user;
    }
}