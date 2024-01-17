using Database.Data;
using Database.Models.RoleModels;
using Microsoft.EntityFrameworkCore;

namespace Database.UseCases;

public class Role_UseCases
{
    public static async Task<IEnumerable<RoleModel>> GetRoles()
    {
        return await ManagementSystemDatabaseContext.Context.Roles.AsQueryable().AsNoTracking().Select(s => new RoleModel(s)).ToListAsync();
    }

    public static async Task<RoleModel?> GetRoleById(int id)
    {
        var role = await ManagementSystemDatabaseContext.Context.Roles.AsQueryable().AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return role == null ? null : new RoleModel(role);
    }
}