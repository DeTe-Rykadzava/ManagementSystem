using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.ViewModels.Products.Factories;

public interface IEditProductViewModelFactory
{
    public EditProductViewModel Create(ProductViewModel product);
}