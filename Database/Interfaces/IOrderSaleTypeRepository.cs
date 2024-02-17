using Database.Models.Order;

namespace Database.Interfaces;

public interface IOrderSaleTypeRepository
{
    Task<IEnumerable<OrderSaleTypeModel>> GetAllAsync();
}