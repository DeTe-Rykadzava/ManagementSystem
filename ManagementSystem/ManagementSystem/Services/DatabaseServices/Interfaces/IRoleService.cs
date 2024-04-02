using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Role;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IRoleService
{
    public Task<ActionResultViewModel<IEnumerable<RoleViewModel>>> GetAll();

    public Task<ActionResultViewModel<RoleViewModel>> GetById(int id);

    public Task<ActionResultViewModel<RoleViewModel>> GetByName(string name);
}