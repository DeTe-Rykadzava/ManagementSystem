using Database.Context;
using Database.Interfaces;
using Database.Models.RoleModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IManagementSystemDatabaseContext _context;

    public RoleRepository(IManagementSystemDatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<RoleModel>> GetAll()
    {
        return await _context.Roles.AsQueryable().AsNoTracking().Select(s => new RoleModel(s)).ToListAsync();
    }

    public async Task<RoleModel?> GetById(int id)
    {
        var role = await _context.Roles.AsQueryable().AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return role == null ? null : new RoleModel(role);
    }
    
    public async Task<RoleModel?> GetByName(string name)
    {
        var role = await _context.Roles.AsQueryable().AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleName == name);
        return role == null ? null : new RoleModel(role);
    }
}