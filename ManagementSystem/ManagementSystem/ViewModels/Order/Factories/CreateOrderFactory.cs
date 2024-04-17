using System.Collections.Generic;
using ManagementSystem.ViewModels.DataVM.Product;

namespace ManagementSystem.ViewModels.Order.Factories;

public class CreateOrderFactory : ICreateOrderFactory
{
    public CreateOrderViewModel CreateCreateOrderViewModel(IEnumerable<ProductViewModel> products)
    {
        throw new System.NotImplementedException();
    }
}