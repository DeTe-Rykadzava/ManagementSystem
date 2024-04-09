using Database.Models.Product;

namespace ManagementSystem.ViewModels.DataVM.Product;

public class ProductCategoryViewModel
{
    private readonly ProductCategoryModel _model;
    public int Id => _model.Id;
    public string CategoryName => _model.CategoryName;

    public ProductCategoryViewModel(ProductCategoryModel model)
    {
        _model = model;
    }
}