using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Role;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class RoleService : IRoleService
{
    public Task<IEnumerable<RoleViewModel>> GetAll()
    {
        throw new System.NotImplementedException();
    }

    public Task<RoleViewModel?> GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<RoleViewModel?> GetByName(string name)
    {
        throw new System.NotImplementedException();
    }
}