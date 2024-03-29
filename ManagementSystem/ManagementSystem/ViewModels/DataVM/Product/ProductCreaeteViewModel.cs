using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }
    
    public ObservableCollection<byte[]> Images { get; set; }

    public ActionStatusViewModel<ProductCreateModel> ToBaseCreateModel()
    {
        return new ActionStatusViewModel<ProductCreateModel>(new List<string>(), null);
    }
}