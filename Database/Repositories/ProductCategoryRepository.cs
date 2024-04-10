using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Core;
using Database.Models.Product;
using Database.Models.RoleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IProductCategoryRepository> _logger;
    
    public ProductCategoryRepository(IManagementSystemDatabaseContext context, ILogger<ProductCategoryRepository> logger) => (_context, _logger) = (context, logger);
    
    public async Task<ActionResultModel<IEnumerable<ProductCategoryModel>>> GetAllCategories()
    {
        var result = new ActionResultModel<IEnumerable<ProductCategoryModel>>();
        try
        {
            var products = await _context.ProductCategories.Select(s => new ProductCategoryModel(s)).ToListAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessGet);
            result.Value = products;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get product categories from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<ProductCategoryModel>> AddCategory(string categoryName)
    {
        var result = new ActionResultModel<ProductCategoryModel>();
        try
        {
            var category = new ProductCategory { CategoryName = categoryName};

            await _context.ProductCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessAdd);
            result.ResultTypes.Add(ActionResultType.SuccessSave);
            result.Value = new ProductCategoryModel(category);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with add product category into database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> DeleteCategory(int id)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var category = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                result.IsSuccess = false;
                result.ResultTypes.Add(ActionResultType.FailDelete);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                _context.ProductCategories.Remove(category);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessDelete);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete product category from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }
}