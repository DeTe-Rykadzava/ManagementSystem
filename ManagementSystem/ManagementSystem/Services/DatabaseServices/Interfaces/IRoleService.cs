using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Role;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IRoleService
{
    public Task<IEnumerable<RoleViewModel>> GetAll();

    public Task<RoleViewModel?> GetById(int id);

    public Task<RoleViewModel?> GetByName(string name);
}