using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IManagementSystemDatabaseContext _context;

    private readonly ILogger<OrderRepository> _logger;
    
    public OrderRepository(IManagementSystemDatabaseContext context, ILogger<OrderRepository> logger) => (_context, _logger) = (context, logger);
    
    public async Task<OrderModel?> GetByIdAsync(int id)
    {
        try
        {
            var order = await _context.Orders
                .Include(i => i.PaymentType)
                .FirstOrDefaultAsync(x => x.Id == id);
            return order == null ? null : new OrderModel(order);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get order by Id: {Id} from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<OrderModel?> CreateAsync(OrderCreateModel model)
    {
        try
        {
            var products = await _context.Products.Where(x => model.Products.Any(a => a.ProductId == x.Id)).ToListAsync();
            var order = new Order
            {
                BuyerEmail = model.BuyerEmail,
                Cost = products.Sum(s => s.Cost),
                CreateDate = DateTime.Today,
                StatusUpdateDate = DateTime.Today,
                PaymentTypeId = model.PaymentTypeId,
                UserId = model.UserId,
                TypeSaleId = model.TypeSaleId,
                StatusId = 1
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var product in model.Products)
            {
                var orderComposition = new OrderComposition
                {
                    OrderId = order.Id,
                    ProductId = product.ProductId,
                    ProductCount = product.ProductsCount
                };

                await _context.OrderCompositions.AddAsync(orderComposition);
                await _context.SaveChangesAsync();
            }
            
            return new OrderModel(order);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with add order into database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<bool> UpdateStatusAsync(OrderEditStatusModel model)
    {
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == model.OrderId);
            if (order == null) return false;
            order.StatusId = model.StatusId;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with update status order from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with remove order from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }
}