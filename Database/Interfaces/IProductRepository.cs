using Database.Models.Product;

namespace Database.Interfaces;

public interface IProductRepository
{
    public Task<IEnumerable<ProductModel>> GetProducts();
    public Task<ProductModel?> GetProduct(int id);
    public Task<IEnumerable<ProductModel>?> AddProducts(IEnumerable<ProductCreateModel> products);
    public Task<ProductModel?> AddProduct(ProductCreateModel product);
    public Task<ProductPhotoModel?> AddProductPhoto(ProductPhotoAppendModel photo);
    public Task<ProductModel?> UpdateProduct(ProductEditModel product);
    public Task<bool> DeleteProducts(IEnumerable<int> ids);
    public Task<bool> RemoveProductPhoto(int photoId);
    public Task<bool> DeleteProduct(int id);
}