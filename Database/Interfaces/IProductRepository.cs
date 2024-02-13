using Database.Models.Product;

namespace Database.Interfaces;

public interface IProductRepository
{
    public Task<IEnumerable<ProductModel>> GetProducts();
    public Task<ProductModel?> GetProduct(int id);
    public Task<IEnumerable<ProductModel>?> AddProducts(IEnumerable<ProductCreateModel> products);
    public Task<ProductModel?> AddProduct(ProductCreateModel products);
    public Task<IEnumerable<ProductModel>?> UpdateProducts(IEnumerable<ProductCreateModel> products);
    public Task<ProductModel?> UpdateProduct(ProductCreateModel products);
    public Task<bool> DeleteProducts(IEnumerable<int> ids);
    public Task<bool> DeleteProduct(int id);
}