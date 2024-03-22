using Database.Context;
using Database.Interfaces;
using Database.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class OrderSaleTypeRepository : IOrderSaleTypeRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<OrderSaleTypeRepository> _logger;

    public OrderSaleTypeRepository(IManagementSystemDatabaseContext context, ILogger<OrderSaleTypeRepository> logger) =>
        (_context, _logger) = (context, logger);
    
    public OrderSaleTypeRepository(IManagementSystemDatabaseContext context) => (_context, _logger) = (context, new Logger<OrderSaleTypeRepository>(new LoggerFactory()));
    
    public async Task<IEnumerable<OrderSaleTypeModel>> GetAllAsync()
    {
        try
        {
            return await _context.OrderTypeSales.Select(s => new OrderSaleTypeModel(s)).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get order sale types from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return Array.Empty<OrderSaleTypeModel>();
        }
    }
}