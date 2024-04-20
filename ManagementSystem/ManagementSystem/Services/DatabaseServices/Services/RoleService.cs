using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Role;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly ILogger<IRoleService> _logger;

    public RoleService(IRoleRepository repository, ILogger<RoleService> logger) => (_repository, _logger) = (repository, logger);
    
    public async Task<ActionResultViewModel<IEnumerable<RoleViewModel>>> GetAll()
    {
        var result = new ActionResultViewModel<IEnumerable<RoleViewModel>>();
        try
        {
            var getResult = await _repository.GetAll();
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = getResult.Value.Select(s => new RoleViewModel(s)).ToList();
            }
        }
        catch (Exception e)
        {
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<RoleViewModel>> GetById(int id)
    {
        var result = new ActionResultViewModel<RoleViewModel>();
        try
        {
            var getResult = await _repository.GetById(id);
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = new RoleViewModel(getResult.Value);
            }
        }
        catch (Exception e)
        {
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<RoleViewModel>> GetByName(string name)
    {
        var result = new ActionResultViewModel<RoleViewModel>();
        try
        {
            var getResult = await _repository.GetByName(name);
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = new RoleViewModel(getResult.Value);
            }
        }
        catch (Exception e)
        {
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}