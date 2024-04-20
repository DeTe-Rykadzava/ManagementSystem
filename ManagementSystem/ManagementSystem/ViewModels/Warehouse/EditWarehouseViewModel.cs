using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Database.Models.Warehouse;
using DynamicData;
using ManagementSystem.Services.BasketService;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using ManagementSystem.ViewModels.DataVM.Warehouse;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Warehouse;

public class EditWarehouseViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "edit-warehouse";
    public override INavigationService RootNavManager { get; protected set; } = null!;
    
    // services
    private readonly IWarehouseService _warehouseService;
    private readonly IProductService _productService;
    private readonly IDialogService _dialogService;
    
    // fields
    public WarehouseViewModel Warehouse { get; }

    public ObservableCollection<ProductViewModelMinimalData> Products { get; } = new ();

    public ObservableCollection<WarehouseProductViewModel> ChangedProducts { get; } = new ();

    private bool _warehouseProductsIsEmpty;
    public bool WarehouseProductsIsEmpty
    {
        get => _warehouseProductsIsEmpty;
        private set => this.RaiseAndSetIfChanged(ref _warehouseProductsIsEmpty, value);
    }

    private bool _productsIsEmpty;
    public bool ProductsIsEmpty
    {
        get => _productsIsEmpty;
        private set => this.RaiseAndSetIfChanged(ref _productsIsEmpty, value);
    }

    private string? _status = string.Empty;
    public string? Status
    {
        get => _status;
        private set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    // commands 
    public ReactiveCommand<WarehouseProductViewModel, Unit> RemoveProductFromWarehouseCommand { get; }
    public ReactiveCommand<ProductViewModelMinimalData, Unit> AddProductIntoWarehouseCommand { get; }
    public ICommand SaveChangesProductCountsIntoWarehouseCommand { get; }
    public ICommand CanselCommand { get; }

    public EditWarehouseViewModel(IWarehouseService warehouseService, IProductService productService, IDialogService dialogService, WarehouseViewModel warehouse)
    {
        _warehouseService = warehouseService;
        _productService = productService;
        _dialogService = dialogService;
        Warehouse = warehouse;
        WarehouseProductsIsEmpty = !Warehouse.Products.Any();
        
        RemoveProductFromWarehouseCommand = ReactiveCommand.CreateFromTask(async (WarehouseProductViewModel warehouseProduct) =>
        {
            Status = null;
            var dialogResult =
                await _dialogService.ShowPopupDialogAsync("Question", "Are you sure you want to delete a product from warehouse?", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            var removeResult = await _warehouseService.DeleteProductFromWarehouseAsync(Warehouse.Id, warehouseProduct.Id);
            if (!removeResult.IsSuccess)
            {
                Status = $"The product has not been deleted.Purposes:\n\t *{string.Join("\n\t *", removeResult.Statuses)}";
                await _dialogService.ShowPopupDialogAsync("Error", Status, icon: Icon.Error);
            }
            else
            {
                Status = $"Success remove product from warehouse";
                Warehouse.Products.Remove(warehouseProduct);
                WarehouseProductsIsEmpty = !Warehouse.Products.Any();
            }
        });
        AddProductIntoWarehouseCommand = ReactiveCommand.CreateFromTask(async (ProductViewModelMinimalData product) =>
        {
            Status = null;
            if (Warehouse.Products.FirstOrDefault(x => x.ProductId == product.Id) != null)
            {
                await _dialogService.ShowPopupDialogAsync("Stop", "Product already exist into warehouse", icon: Icon.Stop);
                return;
            }
            var addResult = await _warehouseService.AppendProductToWarehouseAsync(Warehouse.Id, product.Id);
            if (!addResult.IsSuccess || addResult.Value == null)
            {
                Status = $"The product has not been added.Purposes:\n\t *{string.Join("\n\t *", addResult.Statuses)}";
                await _dialogService.ShowPopupDialogAsync("Error", Status, icon: Icon.Error);
            }
            else
            {
                Status = $"Success add product into warehouse";
                Warehouse.Products.Add(addResult.Value);
                Warehouse.Products.Last().PropertyChanged += WarehouseProductChangedCount;
                WarehouseProductsIsEmpty = !Warehouse.Products.Any();
            }
        });
        SaveChangesProductCountsIntoWarehouseCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Status = null;
            var successSaved = new List<WarehouseProductViewModel>();
            foreach (var product in ChangedProducts)
            {
                var updateCountResult =
                    await _warehouseService.UpdateProductCountInWarehouseAsync(Warehouse.Id, product.Id,
                        product.CountOnStock);
                if (!updateCountResult.IsSuccess || updateCountResult.Value == null)
                {
                    Status = "Problem with save count of products";
                    await _dialogService.ShowPopupDialogAsync("Error", $"The quantity of the product in stock could not be updated.Purposes:\n\t *{string.Join("\n\t *", updateCountResult.Statuses)}", icon: Icon.Error);
                }
                else
                {
                    successSaved.Add(product);
                }
            }
            ChangedProducts.RemoveMany(successSaved);
        });
        CanselCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await UnsubscribeOnChanged();
            await RootNavManager.GoBack();
        });
        
    }

    private void SubscribeOnChanges()
    {
        foreach (var product in Warehouse.Products)
        {
            product.PropertyChanged += WarehouseProductChangedCount;
        }
    }
    
    private Task UnsubscribeOnChanged()
    {
        foreach (var product in Warehouse.Products)
        {
            product.PropertyChanged -= WarehouseProductChangedCount;
        }

        return Task.FromResult(new object());
    }

    private void WarehouseProductChangedCount(object? sender, PropertyChangedEventArgs args)
    {
        Dispatcher.UIThread.Invoke(new Action(() =>
        {
            if(sender is WarehouseProductViewModel model)
                ChangedProducts.Add(model);
        }));
    }

    public override async Task OnShowed()
    {
        Task.Run(LoadProducts);
        Task.Run(SubscribeOnChanges);
    }

    private async Task LoadProducts()
    {
        Dispatcher.UIThread.Invoke(new Action(() =>
        {
            Products.Clear();
        }));
        var productsResult = await _productService.GetProductsWithMinimalData();
        if(!productsResult.IsSuccess || productsResult.Value == null)
            return;
        foreach (var product in productsResult.Value)
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Products.Add(product);
            }));
        }
        ProductsIsEmpty = !Products.Any();
    }
}