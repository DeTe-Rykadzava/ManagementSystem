using Database.Models.RoleModels;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Role;

public class RoleViewModel : ViewModelBase
{
    private readonly RoleModel _role;

    public int Id => _role.Id;
    public string RoleName => _role.RoleName;

    public RoleViewModel(RoleModel role)
    {
        _role = role;
    }
}