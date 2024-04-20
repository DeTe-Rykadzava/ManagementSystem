using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Core;
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
    
    public async Task<ActionResultModel<WarehouseModel>> GetWarehouseAsync(int id)
    {
        var result = new ActionResultModel<WarehouseModel>();
        try
        {
            var warehouse = await _context.Warehouses
                .Include(i => i.ProductWarehouses)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (warehouse == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = new WarehouseModel(warehouse);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get warehouse with Id={Id}.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<IEnumerable<WarehouseModel>>> GetWarehousesAsync()
    {
        var result = new ActionResultModel<IEnumerable<WarehouseModel>>();
        try
        { 
            var warehouses = await _context.Warehouses
                .Include(i => i.ProductWarehouses)
                .ThenInclude(i => i.Product)
                .ToListAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessGet);
            result.Value = warehouses.Select(s => new WarehouseModel(s)).ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get warehouses.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<WarehouseModel>> AddWarehouseAsync(string name)
    {
        var result = new ActionResultModel<WarehouseModel>();
        try
        {
            var warehouse = new Warehouse
            {
                Name = name
            };

            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();
            
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessAdd);
            result.ResultTypes.Add(ActionResultType.SuccessSave);
            result.Value = new WarehouseModel(warehouse);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with append warehouse in database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<WarehouseProductModel>> AppendProductToWarehouseAsync(WarehouseManageProductModel model)
    {
        var result = new ActionResultModel<WarehouseProductModel>();
        try
        {
            var productWarehouse = new ProductWarehouse
            {
                ProductId = model.ProductId,
                WarehouseId = model.WarehouseId,
                Count = model.CountProducts
            };

            await _context.ProductWarehouses.AddAsync(productWarehouse);
            await _context.SaveChangesAsync();

            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessAdd);
            result.ResultTypes.Add(ActionResultType.SuccessSave);
            result.Value = new WarehouseProductModel(productWarehouse);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with append product in warehouse.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<WarehouseProductModel>> UpdateProductCountInWarehouseAsync(WarehouseManageProductModel model)
    {
        var result = new ActionResultModel<WarehouseProductModel>();
        try
        {
            var productWarehouse = await _context.ProductWarehouses.FirstOrDefaultAsync(x =>
                x.WarehouseId == model.WarehouseId && x.ProductId == model.ProductId);

            if (productWarehouse == null)
            {
                result.ResultTypes.Add(ActionResultType.FailEdit);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                if(productWarehouse.Count != model.CountProducts)
                    productWarehouse.Count = model.CountProducts;
            
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessEdit);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.Value = new WarehouseProductModel(productWarehouse);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with edit count product in warehouse.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailEdit);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> DeleteProductFromWarehouseAsync(WarehouseManageProductModel model)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var productWarehouse = await _context.ProductWarehouses.FirstOrDefaultAsync(x =>
                x.WarehouseId == model.WarehouseId && x.ProductId == model.ProductId);

            if (productWarehouse == null)
            {
                result.ResultTypes.Add(ActionResultType.FailDelete);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                _context.ProductWarehouses.Remove(productWarehouse);
            
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessDelete);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with remove product from warehouse.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> DeleteWarehouseAsync(int id)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => x.Id == id);

            if (warehouse == null)
            {
                result.ResultTypes.Add(ActionResultType.FailDelete);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                _context.Warehouses.Remove(warehouse);
            
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessDelete);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete warehouse from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }
}