using System.Collections.Generic;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.ViewModels.Order.Factories;

public interface ICreateOrderVmFactory
{
    public CreateOrderViewModel CreateCreateOrderViewModel(IEnumerable<ProductViewModel> products);
}