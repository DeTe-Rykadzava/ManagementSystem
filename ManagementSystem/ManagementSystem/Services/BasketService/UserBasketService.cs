using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Threading;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.UserStorage;
using ManagementSystem.ViewModels.DataVM.Product;
using ManagementSystem.ViewModels.DataVM.User;
using MsBox.Avalonia.Enums;

namespace ManagementSystem.Services.BasketService;

public class UserBasketService : IUserBasketService, INotifyPropertyChanged
{
    // services
    private readonly IUserStorageService _userStorageService;
    private readonly IBasketService _basketService;
    private readonly IDialogService _dialogService;
    private readonly IProductService _productService;
    
    // fields
    public ObservableCollection<ProductViewModel> Products { get; } = new();

    private bool _userBasketProductsIsEmpty = true;
    public bool UserBasketProductsIsEmpty
    {
        get => _userBasketProductsIsEmpty;
        set => Set(ref _userBasketProductsIsEmpty, value);
    }

    public UserBasketService(IUserStorageService userStorageService, IBasketService basketService, IDialogService dialogService, IProductService productService)
    {
        _userStorageService = userStorageService;
        _basketService = basketService;
        _dialogService = dialogService;
        _productService = productService;
        _userStorageService.PropertyChanged += (sender, args) =>
        {
            UserBasketProductsIsEmpty = true;
            if (args.PropertyName != nameof(_userStorageService.CurrentUser))
                return;
            if (_userStorageService.CurrentUser != null)
                Task.Run(LoadUserProducts);
        };
    }

    private async Task LoadUserProducts()
    {
        try
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Products.Clear();
            }));
            var userBasketResult = await _basketService.Get(_userStorageService.CurrentUser!.Id);
            if (!userBasketResult.IsSuccess || userBasketResult.Value == null)
            {
                await _dialogService.ShowPopupDialogAsync("Error", "Sorry cannot get user basket =(", icon: Icon.Error);
                return;
            }

            foreach (var product in userBasketResult.Value.Products)
            {
                var productVmResult = await _productService.GetProduct(product.ProductId);
                if (!productVmResult.IsSuccess || productVmResult.Value == null)
                {
                    await _dialogService.ShowPopupDialogAsync("Error", $"One of the items in the user's basket could not be retrieved.\nPurpose:\n\t *{string.Join("\n\t *", productVmResult.Statuses)}", icon: Icon.Error);
                    continue;
                }
                Dispatcher.UIThread.Invoke(() =>
                {
                    productVmResult.Value.InUserBasket = true;
                    Products.Add(productVmResult.Value);
                    
                });
            }

            UserBasketProductsIsEmpty = !Products.Any();
        }
        catch (Exception)
        {
            return;
        }
    }
    
    public async Task<bool> AddToUserBasket(ProductViewModel product)
    {
        if(_userStorageService.CurrentUser == null)
            return false;
        
        var addResult= await _basketService.AddIntoBasket(_userStorageService.CurrentUser.Id, product.Id);
        if (!addResult.IsSuccess)
            return false;
        
        if(!Products.Contains(product))
            Products.Add(product);
        UserBasketProductsIsEmpty = !Products.Any();
        return true;
    }

    public async Task<bool> RemoveFromUserBasket(ProductViewModel product)
    {
        if(_userStorageService.CurrentUser == null)
            return false;

        var removeResult= await _basketService.RemoveFromBasket(_userStorageService.CurrentUser.Id, product.Id);
        if (!removeResult.IsSuccess)
            return false;
        Products.Remove(product);
        UserBasketProductsIsEmpty = !Products.Any();
        return true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}