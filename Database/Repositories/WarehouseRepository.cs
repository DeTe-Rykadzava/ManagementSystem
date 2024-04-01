using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Warehouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IWarehouseRepository> _logger;

    public WarehouseRepository(IManagementSystemDatabaseContext context, ILogger<WarehouseRepository> logger) =>
        (_context, _logger) = (context, logger);
    
    public async Task<WarehouseModel?> GetWarehouseAsync(int id)
    {
        try
        {
            var warehouse = await _context.Warehouses
                .Include(i => i.ProductWarehouses)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (warehouse == null)
                return null;
            return new WarehouseModel(warehouse);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get warehouse with Id={Id}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<IEnumerable<WarehouseModel>> GetWarehousesAsync()
    {
        try
        {
            return await _context.Warehouses
                .Include(i => i.ProductWarehouses)
                .ThenInclude(i => i.Product)
                .Select(s => new WarehouseModel(s))
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get warehouses.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return Array.Empty<WarehouseModel>();
        }
        
    }

    public async Task<WarehouseModel?> AddWarehouseAsync(string name)
    {
        try
        {
            var warehouse = new Warehouse
            {
                Name = name
            };

            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();
            
            return new WarehouseModel(warehouse);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with append warehouse in database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<bool> AppendProductToWarehouseAsync(WarehouseManageProductModel model)
    {
        try
        {
            var productWarehouse = new ProductWarehouse
            {
                ProductId = model.ProductId,
                WarehouseId = model.WarehouseId,
                Count = model.CountProducts,
                CountReserved = 0
            };

            await _context.ProductWarehouses.AddAsync(productWarehouse);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with append product in warehouse.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> UpdateProductCountInWarehouseAsync(WarehouseManageProductModel model)
    {
        try
        {
            var productWarehouse = await _context.ProductWarehouses.FirstOrDefaultAsync(x =>
                x.WarehouseId == model.WarehouseId && x.ProductId == model.ProductId);

            if (productWarehouse == null)
                return false;
            
            productWarehouse.Count = model.CountProducts;
            
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with edit count product in warehouse.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> DeleteProductFromWarehouseAsync(WarehouseManageProductModel model)
    {
        try
        {
            var productWarehouse = await _context.ProductWarehouses.FirstOrDefaultAsync(x =>
                x.WarehouseId == model.WarehouseId && x.ProductId == model.ProductId);

            if (productWarehouse == null)
                return false;

            _context.ProductWarehouses.Remove(productWarehouse);
            
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with remove product from warehouse.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> DeleteWarehouseAsync(int id)
    {
        try
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => x.Id == id);

            if (warehouse == null)
                return false;

            _context.Warehouses.Remove(warehouse);
            
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete warehouse from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }
}