using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Order;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class OrderPaymentTypeService : IOrderPaymentTypeService
{
    private readonly IOrderPaymentTypeRepository _repository;
    private readonly ILogger<IOrderPaymentTypeService> _logger;

    public OrderPaymentTypeService(IOrderPaymentTypeRepository repository, ILogger<OrderPaymentTypeService> logger) =>
        (_repository, _logger) = (repository, logger);

    public async Task<ActionResultViewModel<IEnumerable<OrderPaymentTypeViewModel>>> GetAllAsync()
    {
        var result = new ActionResultViewModel<IEnumerable<OrderPaymentTypeViewModel>>();
        try
        {
            var typesResult = await _repository.GetAllAsync();
            if (!typesResult.IsSuccess || typesResult.Value == null)
            {
                result.Statuses.Add("Fail get");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success get");
                result.Value = typesResult.Value.Select(s => new OrderPaymentTypeViewModel(s)).ToList();
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with get all order payment types from database.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
    
    public async Task<ActionResultViewModel<OrderPaymentTypeViewModel>> AddTypeAsync(string typeName)
    {
        var result = new ActionResultViewModel<OrderPaymentTypeViewModel>();
        try
        {
            var createResult = await _repository.AddTypeAsync(typeName);
            if (!createResult.IsSuccess || createResult.Value == null)
            {
                result.Statuses.Add("Fail Add");
                result.Statuses.Add("Fail Save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success Add");
                result.Statuses.Add("Success Save");
                result.Value = new OrderPaymentTypeViewModel(createResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with create order payment type into database.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail Add");
            result.Statuses.Add("Fail Save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> RemoveTypeAsync(int typeId)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            var removeResult = await _repository.RemoveTypeAsync(typeId);
            if (!removeResult.IsSuccess || !removeResult.Value)
            {
                result.Statuses.Add("Fail Remove");
                result.Statuses.Add("Fail Save");
            }
            else
            {
                result.IsSuccess = true;
                result.Statuses.Add("Success Remove");
                result.Statuses.Add("Success Save");
                result.Value = removeResult.Value;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Exception with delete order payment type from database.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Fail Remove");
            result.Statuses.Add("Fail Save");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

}