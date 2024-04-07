using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ManagementSystem.Assets;
using ManagementSystem.Services.DatabaseServices.Interfaces;
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
    private readonly IUserStorageService _userStorageService;
    private readonly IStorageService _storageService;
    private readonly ContentControl _appBaseControl;
    
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
    
    // models
    public ProductCreateViewModel Model { get; }
    
    // commands 
    public ICommand CanselCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand AddProductPhotoCommand { get; }


    public CreateProductViewModel(IUserStorageService userStorageService,
                                  IProductService productService,
                                  IStorageService storageService,
                                  ContentControl appBaseControl)
    {
        _productService = productService;
        _userStorageService = userStorageService;
        _storageService = storageService;
        _appBaseControl = appBaseControl;
        Model = new ProductCreateViewModel();
        CanselCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.GoBack();
        });
        SaveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var saveResult = await _productService.AddProduct(Model);
            if (!saveResult.IsSuccess)
            {
                Status = $"Saving failed, reasons:\n\t* {string.Join("\n\t* ", saveResult.Statuses)}";
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Info",
                    "Success", icon: Icon.Success,
                    windowStartupLocation: WindowStartupLocation.CenterOwner);
                await box.ShowAsPopupAsync(_appBaseControl);
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
            var result = await _storageService.OpenFileAsync(options);
            if (!result.IsSuccess || result.Value == null)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Open file result", $"Open file result is false, purpose:\n\t* {string.Join("\n\t* ", result.Statuses)}",
                    ButtonEnum.Ok, Icon.Error, WindowStartupLocation.CenterOwner);
                await box.ShowAsync();
            }
            else
            {
                var fileBinaries = File.ReadAllBytes(result.Value.Path.ToString()); 
                Model.Images.Add(fileBinaries);
            }
        });
    }

    public override async Task OnShowed()
    {
        if (_userStorageService.CurrentUser?.Role != StaticResources.AdminRoleName || _userStorageService.CurrentUser == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Error",
                "Sorry but you cannot create product into system", icon: Icon.Error,
                windowStartupLocation: WindowStartupLocation.CenterOwner);
            await box.ShowAsPopupAsync(_appBaseControl);
            CanUserCreateProduct = false;
            await RootNavManager.GoBack(); 
        }
        else
        {
            CanUserCreateProduct = true;
        }
    }
}