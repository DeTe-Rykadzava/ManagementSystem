using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Basket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<BasketRepository> _logger;

    public BasketRepository(IManagementSystemDatabaseContext context, ILogger<BasketRepository> logger) =>
        (_context, _logger) = (context, logger);
    
    public BasketRepository(IManagementSystemDatabaseContext context) => (_context, _logger) = (context, new Logger<BasketRepository>(new LoggerFactory()));
    
    public async Task<BasketModel?> Get(int userId)
    {
        try
        {
            var basket = await _context.UserBaskets.FirstOrDefaultAsync(x => x.UserId == userId);
            return basket == null ? null : new BasketModel(basket);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user basket from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<bool> CreateBasket(int userId)
    {
        try
        {
            var basket = new UserBasket
            {
                UserId = userId
            };

            await _context.UserBaskets.AddAsync(basket);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user basket from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> AddIntoBasket(ManageProductIntoBasketModel model)
    {
        try
        {
            var basket = await _context.UserBaskets.Include(i => i.BasketProducts)
                .FirstOrDefaultAsync(x => x.UserId == model.UserId);

            if (basket == null)
                return false;

            if (basket.BasketProducts.Any(x => x.ProductId == model.ProductId))
                return true;

            var basketProduct = new BasketProduct
            {
                ProductId = model.ProductId,
                UserBasketId = basket.Id
            };

            await _context.BasketProducts.AddAsync(basketProduct);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user basket from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> RemoveFromBasket(ManageProductIntoBasketModel model)
    {
        try
        {
            var basket = await _context.UserBaskets.Include(i=> i.BasketProducts)
                .FirstOrDefaultAsync(x => x.UserId == model.UserId);

            var basketProduct = basket?.BasketProducts.FirstOrDefault(x => x.ProductId == model.ProductId);

            if (basketProduct == null)
                return false;
            
            _context.BasketProducts.Remove(basketProduct);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get user basket from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }
}