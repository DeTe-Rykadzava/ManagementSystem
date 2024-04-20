using Database.Context;
using Database.Interfaces;
using Database.Models.Core;
using Database.Models.RoleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IRoleRepository> _logger;
    
    public RoleRepository(IManagementSystemDatabaseContext context, ILogger<RoleRepository> logger) => (_context, _logger) = (context, logger);
    
    public async Task<ActionResultModel<IEnumerable<RoleModel>>> GetAll()
    {
        var result = new ActionResultModel<IEnumerable<RoleModel>>();
        try
        {
            var roles = await _context.Roles.AsQueryable().AsNoTracking().Select(s => new RoleModel(s)).ToListAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessGet);
            result.Value = roles;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get roles.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<RoleModel>> GetById(int id)
    {
        var result = new ActionResultModel<RoleModel>();
        try
        {
            var role = await _context.Roles.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            var model = role == null ? null : new RoleModel(role);
            if (model == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);
            }
            else
            {
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = model;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get role with Id = {Id}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }
    
    public async Task<ActionResultModel<RoleModel>> GetByName(string name)
    {
        var result = new ActionResultModel<RoleModel>();
        try
        {
            var role = await _context.Roles.AsQueryable().AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleName == name);
            var model = role == null ? null : new RoleModel(role);
            if (model == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);
            }
            else
            {
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = model;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get role by name: {Name}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", name, e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }
}