using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.Storage;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Products;

public class EditProductViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "edit_product";
    public override INavigationService RootNavManager { get; protected set; } = null!;

    // services
    private readonly IProductService _productService;
    private readonly IProductCategoryService _productCategoryService;
    private readonly IUserStorageService _userStorageService;
    private readonly IStorageService _storageService;
    private readonly IDialogService _dialogService;
    
    // fields 
    private readonly ProductViewModel _currentProduct;
    
    private string? _status = "Product is not initialized";
    public string? Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }
    
    public ObservableCollection<ProductCategoryViewModel> Categories { get; } = new ();

    private string _title = string.Empty;
    [Required(ErrorMessage = "Title is required")]
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    private string _description = string.Empty;
    [Required(ErrorMessage = "Description is required")]
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    private decimal _cost = 0M;
    [Required(ErrorMessage = "Price is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Cost
    {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    private ProductCategoryViewModel? _category;
    [Required(ErrorMessage = "Category is required")]
    public ProductCategoryViewModel? Category
    {
        get => _category;
        set => this.RaiseAndSetIfChanged(ref _category, value);
    }

    private bool _imagesIsEmpty = true;
    public bool ImagesIsEmpty
    {
        get => _imagesIsEmpty;
        set => this.RaiseAndSetIfChanged(ref _imagesIsEmpty, value);
    }

    public ObservableCollection<ProductPhotoViewModel> Images { get; } = new();

    private bool _productIsInitialized = false;

    public bool ProductIsInitialized
    {
        get => _productIsInitialized;
        set => this.RaiseAndSetIfChanged(ref _productIsInitialized, value);
    }

    // commands
    public ICommand CanselCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand AddProductPhotoCommand { get; }
    public ReactiveCommand<ProductPhotoViewModel, Unit> RemoveProductPhotoCommand { get; }
    
    public EditProductViewModel(IUserStorageService userStorageService,
                                IProductService productService,
                                IStorageService storageService,
                                IDialogService dialogService,
                                IProductCategoryService productCategoryService,
                                ProductViewModel product)
    {
        _currentProduct = product;
        _productService = productService;
        _productCategoryService = productCategoryService;
        _userStorageService = userStorageService;
        _storageService = storageService;
        _dialogService = dialogService;
        Images = new ObservableCollection<ProductPhotoViewModel>(_currentProduct.Images.ToList());
        CanselCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
        SaveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (_currentProduct.Id == 0)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Cannot save current edited product. Purpose: product is not initialized", icon: Icon.Error);
                Status = "Cannot save current edited product. Purpose: product is not initialized";
                return;
            }

            var validationContext = new ValidationContext(this);
            var validationResults = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(this, validationContext, validationResults, true);
            if (validationResult == false)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", $"The data has not been verified. check that the information is filled in correctly. Purposes:\n\t *{string.Join("\n\t *", validationResults.Select(s => s.ErrorMessage).ToList())}", icon: Icon.Stop);
                Status =
                    $"The data has not been verified. check that the information is filled in correctly. Purposes:\n\t *{string.Join("\n\t *", validationResults.Select(s => s.ErrorMessage).ToList())}";
                return;
            }

            if (Category == null)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", $"The data has not been verified. check that the information is filled in correctly. Purposes:\n\t *{string.Join("\n\t *", validationResults.Select(s => s.ErrorMessage).ToList())}", icon: Icon.Stop);
                Status =
                    $"The data has not been verified. check that the information is filled in correctly. Purposes:\n\t *{string.Join("\n\t *", validationResults.Select(s => s.ErrorMessage).ToList())}";
                return;
            }

            var model = new ProductEditViewModel
            {
                Id = this._currentProduct.Id,
                Title = this.Title,
                Description = this.Description,
                CategoryId = this.Category.Id,
                Cost = this.Cost
            };
            
            var editResult = await _productService.UpdateProduct(model);
            if (!editResult.IsSuccess)
            {
                Status = $"Saving failed, reasons:\n\t* {string.Join("\n\t* ", editResult.Statuses)}";
            }
            else
            {
                await _dialogService.ShowPopupDialogAsync("Info", "Success", icon: Icon.Success);
                await RootNavManager.GoBack();
            }
        });
        AddProductPhotoCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (_currentProduct.Id == 0)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Cannot add photo to current product. Purpose: product is not initialized", icon: Icon.Error);
                Status = "Cannot add photo to current product. Purpose: product is not initialized";
                return;
            }

            var options = new FilePickerOpenOptions()
            {
                Title = "Select photo",
                AllowMultiple = false,
                FileTypeFilter = new []{ FilePickerFileTypes.ImageAll }
            };
            try
            {
                var result = await _storageService.OpenFileAsync(options);
                if (!result.IsSuccess || result.Value == null)
                {
                    await _dialogService.ShowPopupDialogAsync("Open file result", $"Open file result is false, purpose:\n\t* {string.Join("\n\t* ", result.Statuses)}", icon: Icon.Error);
                }
                else
                {
                    var fileBinaries = File.ReadAllBytes(result.Value.Path.LocalPath);
                    var addPhotoResult = await _productService.AddProductPhoto(_currentProduct.Id, fileBinaries);
                    if (!addPhotoResult.IsSuccess || addPhotoResult.Value == null)
                    {
                        await _dialogService.ShowPopupDialogAsync("Error",$"Cannot add photo to product. Purposes:\n\t* {string.Join("\n\t *", addPhotoResult.Statuses)}");
                        Status =
                            $"Cannot add photo to product. Purposes:\n\t* {string.Join("\n\t *", addPhotoResult.Statuses)}";
                        return;
                    }
                    Images.Add(addPhotoResult.Value); 
                    ImagesIsEmpty = Images.Any();
                }
            }
            catch (Exception)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Sorry, but we have a problem with adding photo to product");
            }
        });
        RemoveProductPhotoCommand = ReactiveCommand.CreateFromTask(async (ProductPhotoViewModel productPhoto) =>
        {
            if (_currentProduct.Id == 0)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Cannot remove photo from current product. Purpose: product is not initialized", icon: Icon.Error);
                Status = "Cannot remove photo from current product. Purpose: product is not initialized";
                return;
            }

            var dialogResult = await _dialogService.ShowPopupDialogAsync("Question",
                "Are you sure? This action is irreversible.", ButtonEnum.YesNo, Icon.Question);
            if (dialogResult == ButtonResult.No)
            {
                return;
            }

            var removeProductPhotoResult = await _productService.RemoveProductPhoto(productPhoto.Id);
            if (!removeProductPhotoResult.IsSuccess || removeProductPhotoResult.Value == false)
            {
                await _dialogService.ShowPopupDialogAsync("Error", $"Sorry, but I couldn't delete the photo. Reasons:\n\t *{string.Join("\n\t *",removeProductPhotoResult.Statuses)}");
                Status =
                    $"Sorry, but I couldn't delete the photo. Reasons:\n\t *{string.Join("\n\t *", removeProductPhotoResult.Statuses)}";
                return;
            }

            Images.Remove(productPhoto);
            ImagesIsEmpty = Images.Any();
        });
        SetProductData();
    }
    
    private void SetProductData()
    {
        Title = _currentProduct.Title;
        Description = _currentProduct.Description;
        Cost = _currentProduct.Cost;
        Status = null;
    }

    public override async Task OnShowed()
    {
        await Task.Run(LoadCategories);
        await Task.Run(SetProductCategory);
    }

    private async Task SetProductCategory()
    {
        if (Categories.Any())
        {
            Category = Categories.FirstOrDefault(x => x.Id == _currentProduct.CategoryId);
        }
        else
        {
            await Task.Run(LoadCategories);
            Category = Categories.FirstOrDefault(x => x.Id == _currentProduct.CategoryId);
        }
    }

    private async Task LoadCategories()
    {
        try
        {
            var categoriesResult = await _productCategoryService.GetAll();
            if (!categoriesResult.IsSuccess || categoriesResult.Value == null || !categoriesResult.Value.Any())
            {
                Dispatcher.UIThread.Invoke(new Action(async() =>
                {
                    if(categoriesResult.Statuses.Any())
                        await _dialogService.ShowPopupDialogAsync("error", $"sorry but categories of product is empty =(. Purposes:\n\t{string.Join("\n\t", categoriesResult.Statuses)}");
                    else
                        await _dialogService.ShowPopupDialogAsync("error", "sorry but categories of product is empty =(");
                }));
                return;
            }

            foreach (var productCategory in categoriesResult.Value)
            {
                Categories.Add(productCategory);
            }            
        }
        catch (Exception e)
        {
            return;
        }
    }
}