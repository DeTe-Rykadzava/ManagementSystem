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

public class OrderStatusService : IOrderStatusService
{
    private readonly IOrderStatusRepository _repository;
    private readonly ILogger<IOrderStatusService> _logger;

    public OrderStatusService(IOrderStatusRepository repository, ILogger<OrderStatusService> logger) =>
        (_repository, _logger) = (repository, logger);
    
    public async Task<ActionResultViewModel<IEnumerable<OrderStatusViewModel>>> GetAllAsync()
    {
        var result = new ActionResultViewModel<IEnumerable<OrderStatusViewModel>>();
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
                result.Value = getResult.Value.Select(s => new OrderStatusViewModel(s)).ToList();
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get all order statuses from database.\nException: {Exception}.\n{InnerException}.", e.Message, e.InnerException);
        }
        return result;
    }
}