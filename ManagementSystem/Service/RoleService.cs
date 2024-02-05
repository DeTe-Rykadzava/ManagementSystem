using Database.Data;
using Database.Models.RoleModels;
using Database.UseCases;

namespace ManagementSystem.Service;

public class RoleService
{
    private readonly ManagementSystemDatabaseContext _context;

    public RoleService(ManagementSystemDatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<RoleModel>> GetRoles()
    {
        return await Role_UseCases.GetRoles(_context);
    }
    
    public async Task<RoleModel?> GetRolesById(int id)
    {
        return await Role_UseCases.GetRoleById(_context, id);
    }
    
    public async Task<RoleModel?> GetRolesByName(string name)
    {
        return await Role_UseCases.GetRoleByName(_context, name);
    }
}