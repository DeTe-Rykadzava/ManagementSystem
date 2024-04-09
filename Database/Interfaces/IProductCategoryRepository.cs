using Database.Models.Core;
using Database.Models.Product;

namespace Database.Interfaces;

public interface IProductCategoryRepository
{
    public Task<ActionResultModel<IEnumerable<ProductCategoryModel>>> GetAllCategories();
}