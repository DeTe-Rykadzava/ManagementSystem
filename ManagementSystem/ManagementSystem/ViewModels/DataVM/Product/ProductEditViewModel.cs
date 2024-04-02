using System.ComponentModel.DataAnnotations;
using Database.Models.Product;
using ManagementSystem.ViewModels.Core;
using ReactiveUI;

namespace ManagementSystem.ViewModels.DataVM.Product;

public class ProductEditViewModel : ViewModelBase
{
    // TODO: need complete VM
    [Required]
    public int Id { get; set; } = 0;

    private string _title = string.Empty;
    [Required]
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    private string _description = string.Empty; 
    [Required]
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    private double _cost = 0D;
    [Required]
    [Range(0D,double.MaxValue)]
    public double Cost
    {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    private int _categoryId = 0;
    [Required]
    public int CategoryId
    {
        get => _categoryId;
        set => this.RaiseAndSetIfChanged(ref _categoryId, value);
    }

    public ProductEditModel ToBaseModel()
    {
        return new ProductEditModel
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            CategoryId = this.CategoryId,
            Cost = (decimal)this.Cost
        };
    }
}