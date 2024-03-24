using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IProductService
{
    public Task<IEnumerable<ProductViewModel>> GetProducts();
    public Task<ProductViewModel?> GetProduct(int id);
    public Task<ProductViewModel?> AddProduct(ProductCreateViewModel product);
    public Task<ProductPhotoViewModel?> AddProductPhoto(int productId, byte[] image);
    public Task<ProductViewModel?> UpdateProduct(ProductEditViewModel product);
    public Task<bool> DeleteProducts(IEnumerable<int> ids);
    public Task<bool> RemoveProductPhoto(int photoId);
    public Task<bool> DeleteProduct(int id);
}