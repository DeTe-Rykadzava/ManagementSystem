using Database.Context;
using Database.Interfaces;
using Database.Models.RoleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IRoleRepository> _logger;
    
    public RoleRepository(IManagementSystemDatabaseContext context, ILogger<IRoleRepository> logger) => (_context, _logger) = (context, logger);
    
    public async Task<IEnumerable<RoleModel>> GetAll()
    {
        try
        {
            return await _context.Roles.AsQueryable().AsNoTracking().Select(s => new RoleModel(s)).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get roles.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return Array.Empty<RoleModel>();
        }
    }

    public async Task<RoleModel?> GetById(int id)
    {
        try
        {
            var role = await _context.Roles.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return role == null ? null : new RoleModel(role);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get role with Id = {Id}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            return null;
        }
        
    }
    
    public async Task<RoleModel?> GetByName(string name)
    {
        try
        {
            var role = await _context.Roles.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleName == name);
            return role == null ? null : new RoleModel(role);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get role by name: {Name}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", name, e.Message, e.InnerException);
            return null;
        }
    }
}