using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IManagementSystemDatabaseContext _databaseContext;
    
    public ProductRepository(IManagementSystemDatabaseContext databaseContext)=> _databaseContext = databaseContext;
    
    public async Task<IEnumerable<ProductModel>> GetProducts()
    {
        return await _databaseContext.Products.Select(s => new ProductModel(s)).ToListAsync();
    }

    public async Task<ProductModel?> GetProduct(int id)
    {
        var product = await _databaseContext.Products.FirstOrDefaultAsync(s => s.Id == id);
        return product == null ? null : new ProductModel(product);
    }

    public async Task<IEnumerable<ProductModel>?> AddProducts(IEnumerable<ProductCreateModel> products)
    {
        // TODO: Add functionality to add products
        return null;
    }

    public async Task<ProductModel?> AddProduct(ProductCreateModel products)
    {
        // TODO: Add functionality to add product
        return null;
    }

    public async Task<IEnumerable<ProductModel>?> UpdateProducts(IEnumerable<ProductCreateModel> products)
    {
        // TODO: Add functionality to update products
        return null;
    }

    public async Task<ProductModel?> UpdateProduct(ProductCreateModel products)
    {
        // TODO: Add functionality to update product
        return null;
    }

    public async Task<bool> DeleteProducts(IEnumerable<int> ids)
    {
        try
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _databaseContext.Products.FirstOrDefaultAsync(s => s.Id == id);
                    if(product!= null)
                        products.Add(product);
            }

            _databaseContext.Products.RemoveRange(products);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> DeleteProduct(int id)
    {
        try
        {
            var product = await _databaseContext.Products.FirstOrDefaultAsync(s => s.Id == id);
            if (product != null)
            {
                _databaseContext.Products.Remove(product);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}