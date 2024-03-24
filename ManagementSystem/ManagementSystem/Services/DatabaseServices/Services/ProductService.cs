using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.Services.DatabaseServices.Services;

public class ProductService : IProductService
{
    public Task<IEnumerable<ProductViewModel>> GetProducts()
    {
        throw new System.NotImplementedException();
    }

    public Task<ProductViewModel?> GetProduct(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<ProductViewModel?> AddProduct(ProductCreateViewModel product)
    {
        throw new System.NotImplementedException();
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