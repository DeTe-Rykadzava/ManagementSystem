using Database.Models.Product;
using ManagementSystem.ViewModels.Core;

namespace ManagementSystem.ViewModels.DataVM.Product;

public class ProductPhotoViewModel : ViewModelBase
{
    private ProductPhotoModel _productPhoto;

    public int Id => _productPhoto.Id;

    public byte[] Image => _productPhoto.Image;

    public ProductPhotoViewModel(ProductPhotoModel productPhoto)
    {
        _productPhoto = productPhoto;
    }
}