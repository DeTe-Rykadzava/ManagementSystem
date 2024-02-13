using Database.Interfaces;
using Database.Models.RoleModels;
using Database.Repositories;

namespace ManagementSystem.Service;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    
    public async Task<IEnumerable<RoleModel>> GetRoles()
    {
        return await _roleRepository.GetAll();
    }
    
    public async Task<RoleModel?> GetRolesById(int id)
    {
        return await _roleRepository.GetById(id);
    }
    
    public async Task<RoleModel?> GetRolesByName(string name)
    {
        return await _roleRepository.GetByName(name);
    }
}