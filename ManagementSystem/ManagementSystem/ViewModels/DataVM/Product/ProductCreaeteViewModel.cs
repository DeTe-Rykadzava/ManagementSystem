using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Database.Models.Product;
using ManagementSystem.ViewModels.Core;
using ReactiveUI;

namespace ManagementSystem.ViewModels.DataVM.Product;

public class ProductCreateViewModel : ViewModelBase
{
    // TODO: need complete this VM
    
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

    private decimal _cost;
    [Required(ErrorMessage = "Price is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Cost
    {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    private int _categoryId;
    [Required(ErrorMessage = "Category is required")]
    public int CategoryId
    {
        get => _categoryId;
        set => this.RaiseAndSetIfChanged(ref _categoryId, value);
    }

    private bool _imagesIsEmpty = true;
    public bool ImagesIsEmpty
    {
        get => _imagesIsEmpty;
        set => this.RaiseAndSetIfChanged(ref _imagesIsEmpty, value);
    }
    
    public ObservableCollection<byte[]> Images { get; } = new();

    public ProductCreateViewModel()
    {
        Images.CollectionChanged += (sender, args) =>
        {
            ImagesIsEmpty = !Images.Any();
        };
    }
    
    public ProductCreateModel ToBaseModel()
    {
        return new ProductCreateModel
        {
            Cost = this.Cost,
            Description = this.Description,
            CategoryId = this.CategoryId,
            Title = this.Title,
            Images = this.Images.ToList()
        };
    }
}