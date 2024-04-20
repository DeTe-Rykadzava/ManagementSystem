using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

    private OrderSaleTypeViewModel? _typeSale;
    [Required(ErrorMessage = "Type of sale cannot be empty")]
    public OrderSaleTypeViewModel? TypeSale
    {
        get => _typeSale;
        set => this.RaiseAndSetIfChanged(ref _typeSale, value);
    }

    private OrderPaymentTypeViewModel? _paymentType;
    [Required(ErrorMessage = "Type of payment cannot be empty")]
    public OrderPaymentTypeViewModel? PaymentType
    {
        get => _paymentType;
        set => this.RaiseAndSetIfChanged(ref _paymentType, value);
    }

    public int? UserId { get; set; } = null;

    private ObservableCollection<OrderProductCreateViewModel> _products = new();
    public ObservableCollection<OrderProductCreateViewModel> Products
    {
        get => _products;
        set => this.RaiseAndSetIfChanged(ref _products, value);
    }

    public async Task<ActionResultViewModel<bool>> CheckValid()
    {
        var result = new ActionResultViewModel<bool>();

        var validationContext = new ValidationContext(this);
        var validationResults = new List<ValidationResult>();
        var validationResult = Validator.TryValidateObject(this, validationContext, validationResults, true);
        if (validationResult)
        {
            result.IsSuccess = true;
            result.Statuses.Add("Success validate");
            result.Value = true;
        }
        else
        {
            if(validationResults.Any())
                result.Statuses.AddRange(validationResults.Select(s => s.ErrorMessage!).ToList());
        }
        return result;
    }
}