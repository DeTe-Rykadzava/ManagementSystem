using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models.Warehouse;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Warehouse;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _repository;
    private readonly ILogger<IWarehouseService> _logger;

    public WarehouseService(IWarehouseRepository repository, ILogger<WarehouseService> logger) =>
        (_repository, _logger) = (repository, logger);
    
    public async Task<ActionResultViewModel<WarehouseViewModel>> GetWarehouseAsync(int id)
    {
        var result = new ActionResultViewModel<WarehouseViewModel>();
        try
        {
            var getResult = await _repository.GetWarehouseAsync(id);
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = new WarehouseViewModel(getResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with get warehouse by id {Id}.\nException: {Message}.\nInnerException: {InnerException}",id, e.Message, e.InnerException);
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<IEnumerable<WarehouseViewModel>>> GetWarehousesAsync()
    {
        var result = new ActionResultViewModel<IEnumerable<WarehouseViewModel>>();
        try
        {
            var getResult = await _repository.GetWarehousesAsync();
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = getResult.Value.Select(s => new WarehouseViewModel(s)).ToList();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with all warehouses.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<WarehouseViewModel>> AddWarehouseAsync(string name)
    {
        var result = new ActionResultViewModel<WarehouseViewModel>();
        try
        {
            var addResult = await _repository.AddWarehouseAsync(name);
            if (!addResult.IsSuccess || addResult.Value == null)
            {
                result.Statuses.Add("Fail add");
                result.Statuses.Add("Fail save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success add");
                result.Statuses.Add("Success save");
                result.Value = new WarehouseViewModel(addResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with add warehouses into database.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail add");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<WarehouseProductViewModel>> AppendProductToWarehouseAsync(int warehouseId, int productId)
    {
        var result = new ActionResultViewModel<WarehouseProductViewModel>();
        try
        {
            var model = new WarehouseManageProductModel
            {
                WarehouseId = warehouseId,
                ProductId = productId
            };
            
            var addResult = await _repository.AppendProductToWarehouseAsync(model);
            if (!addResult.IsSuccess || addResult.Value == null)
            {
                result.Statuses.Add("Fail add");
                result.Statuses.Add("Fail save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success add");
                result.Statuses.Add("Success save");
                result.Value = new WarehouseProductViewModel(addResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with add product into warehouse.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail add");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<WarehouseProductViewModel>> UpdateProductCountInWarehouseAsync(int warehouseId, int productId, int productCount)
    {
        var result = new ActionResultViewModel<WarehouseProductViewModel>();
        try
        {
            var model = new WarehouseManageProductModel
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                CountProducts = productCount
            };
            
            var updateResult = await _repository.UpdateProductCountInWarehouseAsync(model);
            if (!updateResult.IsSuccess || updateResult.Value == null)
            {
                result.Statuses.Add("Fail update");
                result.Statuses.Add("Fail save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success update");
                result.Statuses.Add("Success save");
                result.Value = new WarehouseProductViewModel(updateResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with update product count on warehouse.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail update");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> DeleteProductFromWarehouseAsync(int warehouseId, int productId)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            var model = new WarehouseManageProductModel
            {
                WarehouseId = warehouseId,
                ProductId = productId
            };
            
            var deleteResult = await _repository.DeleteProductFromWarehouseAsync(model);
            if (!deleteResult.IsSuccess || !deleteResult.Value)
            {
                result.Statuses.Add("Fail delete");
                result.Statuses.Add("Fail save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success delete");
                result.Statuses.Add("Success save");
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with remove product from warehouse.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail delete");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> DeleteWarehouseAsync(int id)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            var deleteResult = await _repository.DeleteWarehouseAsync(id);
            if (!deleteResult.IsSuccess || !deleteResult.Value)
            {
                result.Statuses.Add("Fail delete");
                result.Statuses.Add("Fail save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success delete");
                result.Statuses.Add("Success save");
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with remove warehouse from database.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail delete");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}