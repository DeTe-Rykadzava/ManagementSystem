using Database.DataDatabase;

namespace Database.Models.UserModels;

public class UserModel
{
    public int Id { get; set; }
    
    public int UserInfoId { get; set; }
    
    public string Login { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Patronymic { get; set; }

    public int RoleId { get; set; }

    public Role Role { get; set; }

    public UserModel(User user)
    {
        Id = user.Id;
        UserInfoId = user.UserInfo.Id;
        Login = user.Login;
        FirstName = user.UserInfo.FirstName;
        LastName = user.UserInfo.LastName;
        Patronymic = user.UserInfo.Patronymic;
        RoleId = user.UserInfo.RoleId;
        Role = user.UserInfo.Role;
    }

    public UserModel()
    {
        FirstName = "test";
    }
}