using Database.Context;
using Database.DataDatabase;
using Database.Interfaces;
using Database.Models.Core;
using Database.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IManagementSystemDatabaseContext _context;
    private readonly ILogger<IProductRepository> _logger;

    public ProductRepository(IManagementSystemDatabaseContext context, ILogger<ProductRepository> logger) =>
        (_context, _logger) = (context, logger);
    
    public async Task<ActionResultModel<IEnumerable<ProductModel>>> GetProducts()
    {
        var result = new ActionResultModel<IEnumerable<ProductModel>>();
        try
        {
            var products = await _context.Products.Include(i => i.Category)
                                                                  .Include(i => i.ProductPhotos)
                                                                  .Select(s => new ProductModel(s))
                                                                  .ToListAsync();
            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessGet);
            result.Value = products;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get products from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }
        return result;
    }

    public async Task<ActionResultModel<ProductModel>> GetProduct(int id)
    {
        var result = new ActionResultModel<ProductModel>();
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            if (product == null)
            {
                result.ResultTypes.Add(ActionResultType.FailGet);    
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);    
            }
            else
            {
                result.IsSuccess = true;
                result.ResultTypes.Add(ActionResultType.SuccessGet);
                result.Value = new ProductModel(product);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with get product by Id: {Id} from database.\nException:\t{Message}.\nInner Exception:\t{InnerException}", id, e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailGet);
        }

        return result;
    }

    public async Task<ActionResultModel<ProductModel>> AddProduct(ProductCreateModel product)
    {
        var result = new ActionResultModel<ProductModel>();
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
            result.ResultTypes.Add(ActionResultType.SuccessAdd);
            await _context.SaveChangesAsync();
            result.ResultTypes.Add(ActionResultType.SuccessSave);
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

            result.IsSuccess = true;
            result.ResultTypes.Add(ActionResultType.SuccessAdd);
            result.ResultTypes.Add(ActionResultType.SuccessSave);
            result.Value = new ProductModel(newProduct);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with add product in database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
        }

        return result;
    }

    public async Task<ActionResultModel<ProductPhotoModel>> AddProductPhoto(ProductPhotoAppendModel photoModel)
    {
        var result = new ActionResultModel<ProductPhotoModel>();
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == photoModel.ProductId);

            if (product == null)
            {
                result.ResultTypes.Add(ActionResultType.FailAdd);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                var photo = new ProductPhoto { ProductId = product.Id, Image = photoModel.Image };
            
                await _context.ProductPhotos.AddAsync(photo);
                await _context.SaveChangesAsync();
            
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete photo of product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailAdd);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }

        return result;
    }

    public async Task<ActionResultModel<ProductModel>> UpdateProduct(ProductEditModel product)
    {
        var result = new ActionResultModel<ProductModel>();
        try
        {
            var dbProduct = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == product.Id);

            if (dbProduct == null)
            {
                _logger.LogError("Product for edit with Id={Id} not found in database", product.Id);
                result.ResultTypes.Add(ActionResultType.FailEdit);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                if (dbProduct.Title.Trim() != product.Title.Trim())
                {
                    dbProduct.Title = product.Title;
                }
                
                if (dbProduct.Description.Trim() != product.Description.Trim())
                {
                    dbProduct.Description = product.Description;
                }
                
                if (dbProduct.Cost != product.Cost)
                {
                    dbProduct.Cost = product.Cost;
                }
    
                if (dbProduct.CategoryId != product.CategoryId)
                {
                    dbProduct.CategoryId = product.CategoryId;
                }
                
                await _context.SaveChangesAsync();
                
                result.ResultTypes.Add(ActionResultType.SuccessEdit);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.IsSuccess = true;
                result.Value = new ProductModel(dbProduct);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with edit product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailEdit);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }

        return result;
    }

    public async Task<ActionResultModel<bool>> DeleteProducts(IEnumerable<int> ids)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
                    if(product != null)
                        products.Add(product);
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            
            result.ResultTypes.Add(ActionResultType.SuccessDelete);
            result.ResultTypes.Add(ActionResultType.SuccessSave);
            result.IsSuccess = true;
            result.Value = true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete products from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> RemoveProductPhoto(int photoId)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var photo = await _context.ProductPhotos.FirstOrDefaultAsync(x => x.Id == photoId);

            if (photo == null)
            {
                result.ResultTypes.Add(ActionResultType.FailDelete);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
            else
            {
                _context.ProductPhotos.Remove(photo);
                await _context.SaveChangesAsync();
                result.ResultTypes.Add(ActionResultType.SuccessDelete);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.IsSuccess = true;
                result.Value = true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete photo of product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }
        return result;
    }

    public async Task<ActionResultModel<bool>> DeleteProduct(int id)
    {
        var result = new ActionResultModel<bool>();
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                result.ResultTypes.Add(ActionResultType.SuccessDelete);
                result.ResultTypes.Add(ActionResultType.SuccessSave);
                result.IsSuccess = true;
                result.Value = true;
            }
            else
            {
                result.ResultTypes.Add(ActionResultType.FailDelete);
                result.ResultTypes.Add(ActionResultType.ObjectNotExist);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error with delete product from database.\n{Message}.\n{InnerException}", e.Message, e.InnerException);
            result.ResultTypes.Add(ActionResultType.FailDelete);
            result.ResultTypes.Add(ActionResultType.FailSave);
        }

        return result;
    }
}
