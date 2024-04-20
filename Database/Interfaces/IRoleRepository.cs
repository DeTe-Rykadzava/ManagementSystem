using Database.Models.Core;
using Database.Models.RoleModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Interfaces;

public interface IRoleRepository
{
    public Task<ActionResultModel<IEnumerable<RoleModel>>> GetAll();

    public Task<ActionResultModel<RoleModel>> GetById(int id);

    public Task<ActionResultModel<RoleModel>> GetByName(string name);
}