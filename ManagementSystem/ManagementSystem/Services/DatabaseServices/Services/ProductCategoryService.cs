using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository _repository;
    private readonly ILogger<IProductCategoryService> _logger;

    public ProductCategoryService(IProductCategoryRepository repository, ILogger<ProductCategoryService> logger) =>
        (_repository, _logger) = (repository, logger);
    
    public async Task<ActionResultViewModel<IEnumerable<ProductCategoryViewModel>>> GetAll()
    {
        var result = new ActionResultViewModel<IEnumerable<ProductCategoryViewModel>>();
        try
        { 
            var productsResult = await _repository.GetAllCategories();
            if (!productsResult.IsSuccess || productsResult.Value == null)
            {
                result.Statuses.Add("Failed get");
                result.Statuses.Add("Products is null");
            }
            else
            {
                var productVms = productsResult.Value.Select(s => new ProductCategoryViewModel(s)).ToList();
                result.Value = productVms;
                result.IsSuccess = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with get all product categories.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}