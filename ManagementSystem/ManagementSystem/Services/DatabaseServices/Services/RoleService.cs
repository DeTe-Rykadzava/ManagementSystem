using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Role;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class RoleService : IRoleService
{
    public Task<ActionResultViewModel<IEnumerable<RoleViewModel>>> GetAll()
    {
        throw new System.NotImplementedException();
    }

    public Task<ActionResultViewModel<RoleViewModel>> GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<ActionResultViewModel<RoleViewModel>> GetByName(string name)
    {
        throw new System.NotImplementedException();
    }
}