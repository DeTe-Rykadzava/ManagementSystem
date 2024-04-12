using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using ManagementSystem.Assets;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.Storage;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Splat.ModeDetection;

namespace ManagementSystem.ViewModels.Products;

public class CreateProductViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "create_product";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IProductService _productService;
    private readonly IProductCategoryService _productCategoryService;
    private readonly IUserStorageService _userStorageService;
    private readonly IStorageService _storageService;
    private readonly IDialogService _dialogService;
    
    // fields
    private bool _canUserCreateProduct = false;
    public bool CanUserCreateProduct
    {
        get => _canUserCreateProduct;
        set => this.RaiseAndSetIfChanged(ref _canUserCreateProduct, value);
    }

    private string? _status;
    public string? Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public ObservableCollection<ProductCategoryViewModel> Categories { get; } = new ();
    
    // models
    public ProductCreateViewModel Model { get; }
    
    // commands 
    public ICommand CanselCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand AddProductPhotoCommand { get; }


    public CreateProductViewModel(IUserStorageService userStorageService,
                                  IProductService productService,
                                  IStorageService storageService,
                                  IDialogService dialogService,
                                  IProductCategoryService productCategoryService)
    {
        _productService = productService;
        _productCategoryService = productCategoryService;
        _userStorageService = userStorageService;
        _storageService = storageService;
        _dialogService = dialogService;
        Model = new ProductCreateViewModel();
        CanselCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
        SaveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var modelIsValidResult = Model.IsValid();
            if (!modelIsValidResult.IsSuccess)
            {
                Status = $"Saving failed, reasons:\n\t* {string.Join("\n\t* ", modelIsValidResult.Statuses)}";
                return;
            }

            var saveResult = await _productService.AddProduct(Model);
            if (!saveResult.IsSuccess)
            {
                Status = $"Saving failed, reasons:\n\t* {string.Join("\n\t* ", saveResult.Statuses)}";
            }
            else
            {
                await _dialogService.ShowPopupDialogAsync("Info", "Success", icon: Icon.Success);
                await RootNavManager.GoBack();
            }
        });
        AddProductPhotoCommand = ReactiveCommand.CreateFromTask(async () =>
        {
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
                    Model.Images.Add(fileBinaries);
                }
            }
            catch (Exception)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Sorry, but we have a problem with adding photo to product");
            }
        });
    }

    public override async Task OnShowed()
    {
        await Task.Run(LoadCategories);
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
                        await _dialogService.ShowPopupDialogAsync("error", $"sorry but categories of product is empty =( Purposes:\n\t{string.Join("\n\t", categoriesResult.Statuses)}");
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