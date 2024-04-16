using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.Storage;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.ViewModels.Products.Factories;

public class EditProductViewModelFactory : IEditProductViewModelFactory
{
    // services
    private readonly IUserStorageService _userStorageService;
    private readonly IProductService _productService;
    private readonly IStorageService _storageService;
    private readonly IDialogService _dialogService;
    private readonly IProductCategoryService _productCategoryService;

    public EditProductViewModelFactory(IUserStorageService userStorageService,
                                        IProductService productService,
                                        IStorageService storageService,
                                        IDialogService dialogService,
                                        IProductCategoryService productCategoryService)
    {
        _userStorageService = userStorageService;
        _productService = productService;
        _storageService = storageService;
        _dialogService = dialogService;
        _productCategoryService = productCategoryService;
    }
    
    
    public EditProductViewModel Create(ProductViewModel product)
    {
        return new EditProductViewModel(_userStorageService, _productService, _storageService, _dialogService,
            _productCategoryService, product);
    }
}