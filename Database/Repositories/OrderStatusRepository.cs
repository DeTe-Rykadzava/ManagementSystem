using Database.Context;
using Database.Interfaces;
using Database.Models.Core;
using Database.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class OrderStatusRepository : IOrderStatusRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IOrderStatusRepository> _logger;

    public OrderStatusRepository(IManagementSystemDatabaseContext context, ILogger<OrderStatusRepository> logger) =>
        (_context, _logger) = (context, logger);
    
    public async Task<ActionResultModel<IEnumerable<OrderStatusModel>>> GetAllAsync()
    {
        var result = new ActionResultModel<IEnumerable<OrderStatusModel>>();
        try
        {
            var statuses = await _context.OrderStatuses.ToListAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessGet);
            result.Value = statuses.Select(s => new OrderStatusModel(s)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with get all order statuses from database.\nException: {Exception}.\nInnerException: {InnerException}", 
                e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }
}