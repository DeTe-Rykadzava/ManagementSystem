using Database.Models.Core;
using Database.Models.Product;

namespace Database.Interfaces;

public interface IProductCategoryRepository
{
    public Task<ActionResultModel<IEnumerable<ProductCategoryModel>>> GetAllCategories();
    public Task<ActionResultModel<ProductCategoryModel>> AddCategory(string categoryName);

    public Task<ActionResultModel<bool>> DeleteCategory(int id);
}