using Database.Models.RoleModels;

namespace Database.Models.UserModels;

public class NewUserInfoModel
{
    public string UserId { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string? Patronymic { get; set; } = null;

    public RoleModel? Role { get; set; } = null;
}