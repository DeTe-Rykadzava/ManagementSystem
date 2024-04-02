using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Products;

public class ProductsViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "products";
    public override INavigationService RootNavManager { get; protected set; } = null!;

    // services
    private readonly IProductService _productService;
    public IUserStorageService UserStorageService { get; }
    
    // fields
    public ObservableCollection<ProductViewModel> Products { get; } = new ObservableCollection<ProductViewModel>();

    private bool _productsIsEmpty = true;
    public bool ProductsIsEmpty
    {
        get => _productsIsEmpty;
        set => this.RaiseAndSetIfChanged(ref _productsIsEmpty, value);
    }
    
    // commands
    public ICommand CreateProductCommand { get; }
    public ReactiveCommand<ProductViewModel, Unit> DeleteProductCommand { get; }

    public ProductsViewModel(IUserStorageService userStorageService, IProductService productService)
    {
        _productService = productService;
        UserStorageService = userStorageService;
        CreateProductCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await RootNavManager.NavigateTo<CreateProductViewModel>();
        });
        DeleteProductCommand = ReactiveCommand.CreateFromTask(async (ProductViewModel product) => { });
        Task.Run(LoadProducts);
    }

    public override Task OnShowed()
    {
        Task.Run(LoadProducts);
        return Task.FromResult(Unit.Default);
    }

    private async Task LoadProducts()
    {
        var productsResult = await _productService.GetProducts();
        if(!productsResult.IsSuccess || productsResult.Value == null)
            return;
        foreach (var product in productsResult.Value)
        {
            Products.Add(product);
        }
        ProductsIsEmpty = Products.Count == 0;
    }
}