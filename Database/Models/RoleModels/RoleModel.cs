using Database.DataDatabase;

namespace Database.Models.RoleModels;

public class RoleModel
{
    private readonly Role _role;

    public int Id => _role.Id;
    public string RoleName => _role.RoleName;
    
    public RoleModel(Role role)
    {
        _role = role;
    }
}