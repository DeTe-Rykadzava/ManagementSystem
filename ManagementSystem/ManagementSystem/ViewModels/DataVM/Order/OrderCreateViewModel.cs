using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Database.Models.Order;
using ManagementSystem.ViewModels.Core;
using ReactiveUI;

namespace ManagementSystem.ViewModels.DataVM.Order;

public class OrderCreateViewModel : ViewModelBase
{
    private string _buyerEmail = string.Empty;
    
    [Required(ErrorMessage = "Buyer's email cannot be empty", AllowEmptyStrings = false)]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
    public string BuyerEmail
    {
        get => _buyerEmail;
        set => this.RaiseAndSetIfChanged(ref _buyerEmail, value);
    }

    private int _typeSaleId;
    [Required(ErrorMessage = "Type of sale cannot be empty")]
    public int TypeSaleId
    {
        get => _typeSaleId;
        set => this.RaiseAndSetIfChanged(ref _typeSaleId, value);
    }

    private int _paymentTypeId;
    [Required(ErrorMessage = "Type of payment cannot be empty")]
    public int PaymentTypeId
    {
        get => _paymentTypeId;
        set => this.RaiseAndSetIfChanged(ref _paymentTypeId, value);
    }

    public ObservableCollection<OrderProductCreateModel> Products { get; set; } = new();

    public OrderCreateViewModel()
    {
        
    }
}