using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Core;
using Database.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IOrderRepository> _logger;
    
    public OrderRepository(IManagementSystemDatabaseContext context, ILogger<OrderRepository> logger) => (_context, _logger) = (context, logger);

    public async Task<ActionResultModel<IEnumerable<OrderModel>>> GetAllAsync()
    {
        var result = new ActionResultModel<IEnumerable<OrderModel>>();
        try
        {
            var orders = await _context.Orders
                .Include(i => i.Status)
                .Include(i => i.PaymentType)
                .Include(i => i.SaleType)
                .Include(i => i.OrderCompositions)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessGet);
            result.Value = orders.Select(s => new OrderModel(s)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with get all orders from database.\nException: {Exception}.\nInnerException: {InnerException}",
                e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<IEnumerable<OrderModel>>> GetUserAllAsync(int userId)
    {
        var result = new ActionResultModel<IEnumerable<OrderModel>>();
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);
                result.ResultTypes.Add(ActionResultType.NotValidData);
            }
            else
            {
                var orders = await _context.Orders
                    .Include(i => i.Status)
                    .Include(i => i.PaymentType)
                    .Include(i => i.SaleType)
                    .Include(i => i.OrderCompositions)
                    .ThenInclude(i => i.Product)
                    .Where(x => x.BuyerEmail == user.Login || x.UserId == user.Id)
                    .ToListAsync();
    
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = orders.Select(s => new OrderModel(s)).ToList();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Error with get all user orders from database.\nException: {Exception}.\nInnerException: {InnerException}",
                e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }
    
    public async Task<ActionResultModel<OrderModel>> GetByIdAsync(int id)
    {
        var result = new ActionResultModel<OrderModel>();
        try
        {
            var order = await _context.Orders
                .Include(i => i.PaymentType)
                .Include(i => i.SaleType)
                .Include(i => i.Status)
                .Include(i => i.OrderCompositions)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = new OrderModel(order);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get order by Id: {Id} from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    private async Task<decimal> GetProductsCostAsync(List<OrderProductCreateModel> products)
    {
        var cost = 0M;
        try
        {
            foreach (var product in products)
            {
                var dbProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.ProductId);
                if(dbProduct == null)
                    continue;
                cost += dbProduct.Cost * product.ProductsCount;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with calculate of order cost.\nException: {Exception}.\nInnerException: {InnerException}", e.Message, e.InnerException);
        }
        
        return cost;
    }

    public async Task<ActionResultModel<OrderModel>> CreateAsync(OrderCreateModel model)
    {
        var result = new ActionResultModel<OrderModel>();
        try
        {
            var productsCost = await GetProductsCostAsync(model.Products);
            var order = new Order
            {
                BuyerEmail = model.BuyerEmail,
                Cost = productsCost,
                CreateDate = DateTime.Today,
                StatusUpdateDate = DateTime.Today,
                PaymentTypeId = model.PaymentTypeId,
                UserId = model.UserId,
                SaleTypeId = model.TypeSaleId,
                StatusId = 1
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            result.ResultTypes.Add(ActionResultType.SuccessAdd);
            result.ResultTypes.Add(ActionResultType.SuccessSave);
            
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
                result.ResultTypes.Add(ActionResultType.SuccessAdd);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
            }

            var orderFromBd = await GetByIdAsync(order.Id);
            if (!orderFromBd.IsSuccess || orderFromBd.Value == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);
            }
            else
            {
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = orderFromBd.Value;
            }

        }
        catch (Exception e)
        {
            _logger.LogError("Error with add order into database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> UpdateStatusAsync(OrderEditStatusModel model)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == model.OrderId);
            if (order == null)
            {
                result.ResultTypes.Add(ActionResultType.FailEdit);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                order.StatusId = model.StatusId;
                order.StatusUpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessEdit);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with update status order into database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailEdit);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> DeleteAsync(int id)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                result.ResultTypes.Add(ActionResultType.FailDelete);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessDelete);                
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with remove order from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }
}