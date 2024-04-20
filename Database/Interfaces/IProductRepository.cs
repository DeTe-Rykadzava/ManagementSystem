using Database.Models.Core;
using Database.Models.Product;

namespace Database.Interfaces;

public interface IProductRepository
{
    public Task<ActionResultModel<IEnumerable<ProductModel>>> GetProducts();
    public Task<ActionResultModel<IEnumerable<ProductModelMinimalData>>> GetProductsWithMinimalData();
    public Task<ActionResultModel<ProductModel>> GetProduct(int id);
    public Task<ActionResultModel<ProductModel>> AddProduct(ProductCreateModel product);
    public Task<ActionResultModel<ProductPhotoModel>> AddProductPhoto(ProductPhotoAppendModel photo);
    public Task<ActionResultModel<ProductModel>> UpdateProduct(ProductEditModel product);
    public Task<ActionResultModel<bool>> DeleteProducts(IEnumerable<int> ids);
    public Task<ActionResultModel<bool>> RemoveProductPhoto(int photoId);
    public Task<ActionResultModel<bool>> DeleteProduct(int id);
}