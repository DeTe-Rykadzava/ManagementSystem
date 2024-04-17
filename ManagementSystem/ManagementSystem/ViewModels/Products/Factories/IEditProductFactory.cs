using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.ViewModels.Products.Factories;

public interface IEditProductFactory
{
    public EditProductViewModel Create(ProductViewModel product);
}