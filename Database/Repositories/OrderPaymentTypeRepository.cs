using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Core;
using Database.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class OrderPaymentTypeRepository : IOrderPaymentTypeRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IOrderPaymentTypeRepository> _logger;

    public OrderPaymentTypeRepository(IManagementSystemDatabaseContext context, ILogger<OrderPaymentTypeRepository> logger) =>
        (_context, _logger) = (context, logger);
    
    public async Task<ActionResultModel<IEnumerable<OrderPaymentTypeModel>>> GetAllAsync()
    {
        var result = new ActionResultModel<IEnumerable<OrderPaymentTypeModel>>();
        try
        {
            var types = await _context.OrderPaymentTypes.Select(s => new OrderPaymentTypeModel(s)).ToListAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessGet);
            result.Value = types;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get order payment types from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<OrderPaymentTypeModel>> AddTypeAsync(string typeName)
    {
        var result = new ActionResultModel<OrderPaymentTypeModel>();
        try
        {
            var model = new OrderPaymentType
            {
                Type = typeName
            };

            await _context.OrderPaymentTypes.AddAsync(model);
            await _context.SaveChangesAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessAdd);
            result.ResultTypes.Add(ActionResultType.SuccessSave);
            result.Value = new OrderPaymentTypeModel(model);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with add order payment type into database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> RemoveTypeAsync(int typeId)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var model = await _context.OrderPaymentTypes.FirstOrDefaultAsync(x => x.Id == typeId);
            if (model == null)
            {
                result.ResultTypes.Add(ActionResultType.FailDelete);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                _context.OrderPaymentTypes.Remove(model);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessDelete);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with remove order payment type from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }
}