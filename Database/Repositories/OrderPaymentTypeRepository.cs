using Database.Context;
using Database.Interfaces;
using Database.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class OrderPaymentTypeRepository : IOrderPaymentTypeRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<OrderPaymentTypeRepository> _logger;

    public OrderPaymentTypeRepository(IManagementSystemDatabaseContext context, ILogger<OrderPaymentTypeRepository> logger) =>
        (_context, _logger) = (context, logger);
    
    public async Task<IEnumerable<OrderPaymentTypeModel>> GetAllAsync()
    {
        try
        {
            return await _context.OrderPaymentTypes.Select(s => new OrderPaymentTypeModel(s)).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get order payment types from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return Array.Empty<OrderPaymentTypeModel>();
        }
    }
}