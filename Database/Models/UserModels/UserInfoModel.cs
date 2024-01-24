using Database.Data;

namespace Database.Models.UserModels;

public class UserInfoModel
{
    private readonly UserInfo _userInfo;

    public int Id => _userInfo.Id;

    public string FirstName => _userInfo.FirstName;

    public string LastName => _userInfo.LastName;

    public string? Patronymic => _userInfo.Patronymic;

    public string RoleName =>_userInfo.Role.RoleName;
    
    public UserInfoModel(UserInfo userInfo)
    {
        _userInfo = userInfo;
    }
}