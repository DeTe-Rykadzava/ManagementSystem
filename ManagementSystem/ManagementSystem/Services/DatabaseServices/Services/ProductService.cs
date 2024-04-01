using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Product;
using Microsoft.Extensions.Logging;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<IProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger) =>
        (_productRepository, _logger) = (productRepository, logger);
    
    public async Task<IEnumerable<ProductViewModel>> GetProducts()
    {
        try
        {
            var products = await _productRepository.GetProducts();
            var productVms = products.Select(s => new ProductViewModel(s)).ToList();
            return productVms;
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with get all products.\nException: {Message}.\nInnerException: {InnerException}", e.Message, e.InnerException);
            return Array.Empty<ProductViewModel>();
        }
    }

    public async Task<ProductViewModel?> GetProduct(int id)
    {
        try
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
                return null;
            var productVm = new ProductViewModel(product);
            return productVm;
        }
        catch (Exception e)
        {
            _logger.LogError(
                "Exception with get products by Id: {id}.\nException: {Message}.\nInnerException: {InnerException}", id, e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<ProductViewModel?> AddProduct(ProductCreateViewModel product)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }

    public Task<ProductPhotoViewModel?> AddProductPhoto(int productId, byte[] image)
    {
        throw new System.NotImplementedException();
    }

    public Task<ProductViewModel?> UpdateProduct(ProductEditViewModel product)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteProducts(IEnumerable<int> ids)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> RemoveProductPhoto(int photoId)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteProduct(int id)
    {
        throw new System.NotImplementedException();
    }
}