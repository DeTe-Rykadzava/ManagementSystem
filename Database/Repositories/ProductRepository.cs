using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IManagementSystemDatabaseContext _context;

    private readonly ILogger<ProductRepository> _logger;
    
    public ProductRepository(IManagementSystemDatabaseContext databaseContext, ILogger<ProductRepository> logger) => (_context, _logger) = (databaseContext, logger);
    public ProductRepository(IManagementSystemDatabaseContext context) => (_context, _logger) = (context, new Logger<ProductRepository>(new LoggerFactory()));
    
    public async Task<IEnumerable<ProductModel>> GetProducts()
    {
        try
        {
            return await _context.Products.Select(s => new ProductModel(s)).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get products from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return Array.Empty<ProductModel>();
        }
    }

    public async Task<ProductModel?> GetProduct(int id)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            return product == null ? null : new ProductModel(product);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get product by Id: {Id} from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<IEnumerable<ProductModel>?> AddProducts(IEnumerable<ProductCreateModel> products)
    {
        try
        {
            var returnData = new List<ProductModel>();
            foreach (var product in products) 
            {
                try
                {
                    var newProduct = new Product
                    {
                        Cost = product.Cost,
                        Description = product.Description,
                        CategoryId = product.CategoryId,
                        Title = product.Title
                    };
                    await _context.Products.AddAsync(newProduct);
                    await _context.SaveChangesAsync();
                    foreach (var image in product.Images)
                    {
                        var newProductImage = new ProductPhoto
                        {
                            Image = image,
                            ProductId = newProduct.Id
                        };
                        await _context.ProductPhotos.AddAsync(newProductImage);
                        await _context.SaveChangesAsync();
                    }

                    returnData.Add(new ProductModel(newProduct));
                }
                catch (Exception e)
                {
                    _logger.LogError("Error with add product in database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
                    return null;
                }
            }
            return returnData.Count == 0 ? null: returnData;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with add products into database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<ProductModel?> AddProduct(ProductCreateModel product)
    {
        try
        {
            var newProduct = new Product
            {
                Cost = product.Cost,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Title = product.Title
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            foreach (var image in product.Images)
            {
                var newProductImage = new ProductPhoto
                {
                    Image = image,
                    ProductId = newProduct.Id
                };
                await _context.ProductPhotos.AddAsync(newProductImage);
                await _context.SaveChangesAsync();
            }

            return new ProductModel(newProduct);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with add product in database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<ProductPhotoModel?> AddProductPhoto(ProductPhotoAppendModel photoModel)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == photoModel.ProductId);

            if (product == null)
                return null;

            var photo = new ProductPhoto { ProductId = product.Id, Image = photoModel.Image };
            
            await _context.ProductPhotos.AddAsync(photo);
            await _context.SaveChangesAsync();
            
            return new ProductPhotoModel(photo);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete photo of product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<ProductModel?> UpdateProduct(ProductEditModel product)
    {
        try
        {
            var dbProduct = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == product.Id);

            if (dbProduct == null)
            {
                _logger.LogError("Product for edit with Id={Id} not found in database", product.Id);
                return null;
            }

            if (dbProduct.Title.Trim() == product.Title.Trim())
            {
                dbProduct.Title = product.Title;
            }
            
            if (dbProduct.Description.Trim() == product.Description.Trim())
            {
                dbProduct.Description = product.Description;
            }
            
            if (dbProduct.Cost == product.Cost)
            {
                dbProduct.Cost = product.Cost;
            }

            if (dbProduct.CategoryId == product.CategoryId)
            {
                dbProduct.CategoryId = product.CategoryId;
            }
            
            await _context.SaveChangesAsync();
            
            return new ProductModel(dbProduct);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with edit product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            return null;
        }
    }

    public async Task<bool> DeleteProducts(IEnumerable<int> ids)
    {
        try
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
                    if(product!= null)
                        products.Add(product);
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete products from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> RemoveProductPhoto(int photoId)
    {
        try
        {
            var photo = await _context.ProductPhotos.FirstOrDefaultAsync(x => x.Id == photoId);

            if (photo == null)
                return false;

            _context.ProductPhotos.Remove(photo);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete photo of product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }

    public async Task<bool> DeleteProduct(int id)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            return false;
        }
    }
}