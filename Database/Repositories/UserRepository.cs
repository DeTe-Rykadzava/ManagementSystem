using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Core;
using Database.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IUserRepository> _logger;
    
    public UserRepository(IManagementSystemDatabaseContext context, ILogger<UserRepository> logger) => (_context, _logger) = (context, logger);
    
    public async Task<ActionResultModel<UserModel>> GetUserById(int id)
    {
        var result = new ActionResultModel<UserModel>();
        try
        {
            var user = await _context.Users
                .AsQueryable()
                .AsNoTracking()
                .Include(i => i.UserInfo)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                _logger.LogWarning("User with Id: {Id} not founded", id);
                result.ResultTypes.Add(ActionResultType.FailGet);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = new UserModel(user);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user by Id: {Id}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }
    
    public async Task<ActionResultModel<UserModel>> GetUserByLoginPassword(string login, string password)
    {
        var result = new ActionResultModel<UserModel>();
        try
        {
            var user = await _context.Users
                .AsQueryable()
                .AsNoTracking()
                .Include(i => i.UserInfo)
                .ThenInclude(i => i.Role)
                .FirstOrDefaultAsync(x => x.Login == login);
            if (user == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
                {
                    result.ResultTypes.Add(ActionResultType.FailGet);
                    result.ResultTypes.Add(ActionResultType.ObjectNotExist);
                }
                else
                {
                    result.IsSuccess = true;
                    result.ResultTypes.Add(ActionResultType.SuccessGet);
                    result.Value = new UserModel(user);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user by login: {Login} password.\nException:\t{Message}.\nInner Exception:\t{InnerException}",  login, e.Message, e.InnerException); 
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<UserModel>> CreateUser(UserCreateModel model)
    {
        var result = new ActionResultModel<UserModel>();
        try
        {
            if (string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
            {
                result.ResultTypes.Add(ActionResultType.NotValidData);
            }
            else
            {
                var existedUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == model.Login);
                if (existedUser != null)
                {
                    result.ResultTypes.Add(ActionResultType.ConflictData);
                }
                else
                {
                    var userInfo = new UserInfo
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Patronymic = model.Patronymic,
                        RoleId = model.RoleId
                    };
                    
                    var user = new User
                    {
                        Login = model.Login,
                        HashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    };
        
                    await _context.UserInfos.AddAsync(userInfo);
                    await _context.SaveChangesAsync();
                    user.UserInfoId = userInfo.Id;
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    
                    result.Value = new UserModel(user);
                    result.IsSuccess = true;
                    result.ResultTypes.Add(ActionResultType.SuccessAdd);
                    result.ResultTypes.Add(ActionResultType.SuccessSave);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Can not save user in base.\nException:\t{Message}.\nInner Exception:\t{InnerException}",e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }

        return result;
    }
}