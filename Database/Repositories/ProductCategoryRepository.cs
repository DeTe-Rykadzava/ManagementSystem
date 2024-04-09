using Database.Context;
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
}