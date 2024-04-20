using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using ManagementSystem.Services.DatabaseServices.Interfaces;
using ManagementSystem.Services.DialogService;
using ManagementSystem.Services.NavigationService;
using ManagementSystem.ViewModels.Core;
using ManagementSystem.ViewModels.DataVM.Product;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace ManagementSystem.ViewModels.Products;

public class ProductCategoriesViewModel : RoutableViewModelBase
{
    public override string ViewModelViewPath { get; } = "categories";
    public override INavigationService RootNavManager { get; protected set; } = null!;

    // services
    private readonly IProductCategoryService _productCategoryService;
    private readonly IDialogService _dialogService;

    // fields
    private string _newCategoryName = string.Empty;
    [Required]
    public string NewCategoryName
    {
        get => _newCategoryName;
        set => this.RaiseAndSetIfChanged(ref _newCategoryName, value);
    }

    public ObservableCollection<ProductCategoryViewModel> Categories { get; } = new();

    private bool _categoriesEmpty = true;
    public bool CategoriesEmpty
    {
        get => _categoriesEmpty;
        set => this.RaiseAndSetIfChanged(ref _categoriesEmpty, value);
    }
    
    // commands
    public ICommand AddCategoryCommand { get; }
    public ReactiveCommand<ProductCategoryViewModel, Unit> DeleteCategoryCommand { get; }

    public ProductCategoriesViewModel(IProductCategoryService productCategoryService, IDialogService dialogService)
    {
        _productCategoryService = productCategoryService;
        _dialogService = dialogService; 
        var canAddNewCategory = this
            .WhenAnyValue(x => x.NewCategoryName, (categoryName) => !string.IsNullOrWhiteSpace(categoryName))
            .DistinctUntilChanged();
        AddCategoryCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await _productCategoryService.AddCategory(NewCategoryName);
                if (!result.IsSuccess || result.Value == null)
                {
                    await dialogService.ShowPopupDialogAsync("Error", $"Sorry but category was not added. Purpose:\n\t{string.Join("\n\t", result.Statuses)}", icon: Icon.Error);
                }
                else
                {
                    await dialogService.ShowPopupDialogAsync("Success", $"Success add category", icon: Icon.Success);
                    Categories.Add(result.Value);
                    CategoriesEmpty = !Categories.Any();
                }
            },
            canAddNewCategory);
        DeleteCategoryCommand = ReactiveCommand.CreateFromTask(async (ProductCategoryViewModel category) =>
        {
            var dialogResult =
                await _dialogService.ShowPopupDialogAsync("Question", "Are you sure you want to delete a category? In this case, the related products will also be deleted, make sure that the data is saved.", ButtonEnum.YesNo, Icon.Question);
            if(dialogResult == ButtonResult.No)
                return;
            var deleteResult = await _productCategoryService.DeleteCategory(category.Id);
            if (!deleteResult.IsSuccess || deleteResult.Value == false)
            {
                await dialogService.ShowPopupDialogAsync("error", $"sorry but category was not deleted. Purpose:\n\t{string.Join("\n\t", deleteResult.Statuses)}");
                return;
            }
            Categories.Remove(category);
            CategoriesEmpty = !Categories.Any();
        });
    }

    public override async Task OnShowed()
    {
        Task.Run(new Action(async () =>
        {
            Dispatcher.UIThread.Invoke(new Action(() =>
            {
                Categories.Clear();
            }));
            var categoriesResult = await _productCategoryService.GetAll();
            if (!categoriesResult.IsSuccess || categoriesResult.Value == null || !categoriesResult.Value.Any())
            {
                CategoriesEmpty = true;
                return;
            }
            foreach (var category in categoriesResult.Value)
            {
                Dispatcher.UIThread.Invoke(new Action(() =>
                {
                    Categories.Add(category);
                }));
            }
            CategoriesEmpty = !Categories.Any();
        }));
    }
}