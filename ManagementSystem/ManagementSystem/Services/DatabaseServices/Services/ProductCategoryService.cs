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
            var categoriesResult = await _repository.GetAllCategories();
            if (!categoriesResult.IsSuccess || categoriesResult.Value == null)
            {
                result.Statuses.Add("Failed get");
                result.Statuses.Add("Product categories is null");
            }
            else
            {
                var productVms = categoriesResult.Value.Select(s => new ProductCategoryViewModel(s)).ToList();
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

    public async Task<ActionResultViewModel<ProductCategoryViewModel>> AddCategory(string categoryName)
    {
        var result = new ActionResultViewModel<ProductCategoryViewModel>();
        try
        { 
            var addCategoryResult = await _repository.AddCategory(categoryName);
            if (!addCategoryResult.IsSuccess || addCategoryResult.Value == null)
            {
                result.Statuses.Add("Failed add");
                result.Statuses.Add("Fail save category");
            }
            else
            {
                result.Value = new ProductCategoryViewModel(addCategoryResult.Value);
                result.IsSuccess = true;
                result.Statuses.Add("Success add");
                result.Statuses.Add("Success save");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with add product category into system.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed add");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> DeleteCategory(int id)
    {
        var result = new ActionResultViewModel<bool>();
        try
        { 
            var deleteCategoryResult = await _repository.DeleteCategory(id);
            if (!deleteCategoryResult.IsSuccess || deleteCategoryResult.Value == false)
            {
                result.Statuses.Add("Failed delete");
            }
            else
            {
                result.Value = deleteCategoryResult.Value;
                result.IsSuccess = true;
                result.Statuses.Add("Success delete");
                result.Statuses.Add("Success save");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with delete product category from system.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed delete");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}