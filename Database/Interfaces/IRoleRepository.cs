using Database.Data;
using Database.Models.RoleModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Interfaces;

public interface IRoleRepository
{
    public Task<IEnumerable<RoleModel>> GetAll();

    public Task<RoleModel?> GetById(int id);

    public Task<RoleModel?> GetByName(string name);
}