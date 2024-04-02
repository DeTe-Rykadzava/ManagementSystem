using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IProductService
{
    public Task<ActionResultViewModel<IEnumerable<ProductViewModel>>> GetProducts();
    public Task<ActionResultViewModel<ProductViewModel>> GetProduct(int id);
    public Task<ActionResultViewModel<ProductViewModel>> AddProduct(ProductCreateViewModel model);
    public Task<ActionResultViewModel<ProductPhotoViewModel>> AddProductPhoto(int productId, byte[] image);
    public Task<ActionResultViewModel<ProductViewModel>> UpdateProduct(ProductEditViewModel model);
    public Task<ActionResultViewModel<bool>> DeleteProducts(IEnumerable<int> ids);
    public Task<ActionResultViewModel<bool>> RemoveProductPhoto(int photoId);
    public Task<ActionResultViewModel<bool>> DeleteProduct(int id);
}