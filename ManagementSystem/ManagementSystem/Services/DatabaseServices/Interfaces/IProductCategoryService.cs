﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.Services.DatabaseServices.Interfaces;

public interface IProductCategoryService
{
    public Task<ActionResultViewModel<IEnumerable<ProductCategoryViewModel>>> GetAll();
    public Task<ActionResultViewModel<ProductCategoryViewModel>> AddCategory(string categoryName);
    public Task<ActionResultViewModel<bool>> DeleteCategory(int id);
}