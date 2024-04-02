using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models.Product;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<IProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger) =>
        (_productRepository, _logger) = (productRepository, logger);
    
    public async Task<ActionResultViewModel<IEnumerable<ProductViewModel>>> GetProducts()
    {
        var result = new ActionResultViewModel<IEnumerable<ProductViewModel>>();
        try
        { 
            var productsResult = await _productRepository.GetProducts();
            if (!productsResult.IsSuccess || productsResult.Value == null)
            {
                result.Statuses.Add("Failed get");
                result.Statuses.Add("Products is null");
            }
            else
            {
                var productVms = productsResult.Value.Select(s => new ProductViewModel(s)).ToList();
                result.Value = productVms;
                result.IsSuccess = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with get all products.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<ProductViewModel>> GetProduct(int id)
    {
        var result = new ActionResultViewModel<ProductViewModel>();
        try
        {
            var productResult = await _productRepository.GetProduct(id);
            if (productResult.IsSuccess && productResult.Value != null)
            {
                var productVm = new ProductViewModel(productResult.Value);
                result.IsSuccess = true;
                result.Value = productVm;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with get product by Id: {Id}.\nException: {Message}.\nInnerException: {InnerException}", id, e.Message, e.InnerException);
            result.Statuses.Add("Failed get");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<ProductViewModel>> AddProduct(ProductCreateViewModel model)
    {
        var result = new ActionResultViewModel<ProductViewModel>();
        try
        {
            var addResult = await _productRepository.AddProduct(model.ToBaseModel());
            if (addResult.IsSuccess && addResult.Value != null)
            {
                result.IsSuccess = true;
                result.Value = new ProductViewModel(addResult.Value);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with add product.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed add product");
            result.Statuses.Add("Unknown problem");
        }

        return result;
    }

    public async Task<ActionResultViewModel<ProductPhotoViewModel>> AddProductPhoto(int productId, byte[] image)
    {
        var result = new ActionResultViewModel<ProductPhotoViewModel>();
        try
        {
            if (productId == 0 && image.Length == 0)
            {
                result.Statuses.Add("Failed add photo");
                result.Statuses.Add("Input data is not valid");
            }
            else
            {
                var addPhotoResult = await _productRepository.AddProductPhoto(new ProductPhotoAppendModel{ProductId = productId, Image = image});
                if (addPhotoResult.IsSuccess && addPhotoResult.Value != null)
                {
                    result.IsSuccess = true;
                    result.Value = new ProductPhotoViewModel(addPhotoResult.Value);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with add product photo.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed add product photo");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<ProductViewModel>> UpdateProduct(ProductEditViewModel model)
    {
        var result = new ActionResultViewModel<ProductViewModel>();
        try
        {
            if (model.Id == 0)
            {
                result.Statuses.Add("Failed update product");
                result.Statuses.Add("Input data is not valid");
            }
            else
            {
                var updateResult = await _productRepository.UpdateProduct(model.ToBaseModel());
                if (updateResult.IsSuccess && updateResult.Value != null)
                {
                    result.IsSuccess = true;
                    result.Value = new ProductViewModel(updateResult.Value);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with update product.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed update product");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> DeleteProducts(IEnumerable<int> ids)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            if (!ids.Any())
            {
                result.Statuses.Add("Failed delete products");
                result.Statuses.Add("Objects not exist");
            }
            else
            {
                var deleteResult = await _productRepository.DeleteProducts(ids);
                if (deleteResult.IsSuccess && deleteResult.Value != false)
                {
                    result.IsSuccess = true;
                    result.Value = deleteResult.Value;
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with delete products.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed delete products");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> RemoveProductPhoto(int photoId)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            if (photoId == 0)
            {
                result.Statuses.Add("Failed delete product");
                result.Statuses.Add("Input data is not valid");
            }
            else
            {
                var deletePhotoResult = await _productRepository.RemoveProductPhoto(photoId);
                if (deletePhotoResult.IsSuccess && deletePhotoResult.Value != false)
                {
                    result.IsSuccess = true;
                    result.Value = deletePhotoResult.Value;
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with delete product photo.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed remove product photo");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }

    public async Task<ActionResultViewModel<bool>> DeleteProduct(int id)
    {
        var result = new ActionResultViewModel<bool>();
        try
        {
            if (id == 0)
            {
                result.Statuses.Add("Failed delete product");
                result.Statuses.Add("Input data is not valid");
            }
            else
            {
                var deleteProductResult = await _productRepository.DeleteProduct(id);
                if (deleteProductResult.IsSuccess && deleteProductResult.Value != false)
                {
                    result.IsSuccess = true;
                    result.Value = deleteProductResult.Value;
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with delete product.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            result.Statuses.Add("Failed remove product");
            result.Statuses.Add("Unknown problem");
        }
        return result;
    }
}