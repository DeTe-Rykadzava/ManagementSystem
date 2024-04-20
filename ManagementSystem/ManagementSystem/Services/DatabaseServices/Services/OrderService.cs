using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models.Order;
using DynamicData;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<IOrderService> _logger;

    public OrderService(IOrderRepository repository, ILogger<OrderService> logger) =>
        (_repository, _logger) = (repository, logger);
    
    public async Task<ActionResultViewModel<IEnumerable<OrderViewModel>>> GetAllAsync()
    {
        var result = new ActionResultViewModel<IEnumerable<OrderViewModel>>();
        try
        {
            var getResult = await _repository.GetAllAsync();
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = getResult.Value.Select(s => new OrderViewModel(s)).ToList();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with get all orders from database.\nException: {Exception}.\nInnerException: {InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
    
    public async Task<ActionResultViewModel<IEnumerable<OrderViewModel>>> GetUserAllAsync(int userId)
    {
        var result = new ActionResultViewModel<IEnumerable<OrderViewModel>>();
        try
        {
            var getResult = await _repository.GetUserAllAsync(userId);
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = getResult.Value.Select(s => new OrderViewModel(s)).ToList();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with get all user orders from database.\nException: {Exception}.\nInnerException: {InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<OrderViewModel>> GetByIdAsync(int id)
    {
        var result = new ActionResultViewModel<OrderViewModel>();
        try
        {
            var getResult = await _repository.GetByIdAsync(id);
            if (!getResult.IsSuccess || getResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = new OrderViewModel(getResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with get order by id {Id} from database.\nException: {Exception}.\nInnerException: {InnerException}",
                id, e.Message, e.InnerException);
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<OrderViewModel>> CreateAsync(OrderCreateViewModel model)
    {
        var result = new ActionResultViewModel<OrderViewModel>();
        try
        {
            var dbModel = new OrderCreateModel
            {
                BuyerEmail = model.BuyerEmail,
                UserId = model.UserId,
                PaymentTypeId = model.PaymentType!.Id,
                TypeSaleId = model.TypeSale!.Id,
                Products = model.Products.Select(s => new OrderProductCreateModel(s.Product.Id, s.CountOfProduct)).ToList()
            };
            
            var addResult = await _repository.CreateAsync(dbModel);
            if (!addResult.IsSuccess || addResult.Value == null)
            {
                result.Statuses.Add("Fail add");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success add");
                result.Statuses.Add("Success save");
                result.Value = new OrderViewModel(addResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with create order into database.\nException: {Exception}.\nInnerException: {InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail add");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> UpdateStatusAsync(int orderId, int statusId)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            var model = new OrderEditStatusModel
            {
                OrderId = orderId,
                StatusId = statusId
            };

            var editResult = await _repository.UpdateStatusAsync(model);
            if (!editResult.IsSuccess || !editResult.Value)
            {
                result.Statuses.Add("Fail edit");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success edit");
                result.Statuses.Add("Success save");
                result.Value = editResult.Value;
            }

        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with update order into database.\nException: {Exception}.\nInnerException: {InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail edit");
            result.Statuses.Add("fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> DeleteAsync(int id)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            var deleteResult = await _repository.DeleteAsync(id);
            if (!deleteResult.IsSuccess || !deleteResult.Value)
            {
                result.Statuses.Add("Fail edit");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success delete");
                result.Statuses.Add("Success save");
                result.Value = deleteResult.Value;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with delete order from database.\nException: {Exception}.\nInnerException: {InnerException}",
                e.Message, e.InnerException);
            result.Statuses.Add("Fail delete");
            result.Statuses.Add("Fail save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}