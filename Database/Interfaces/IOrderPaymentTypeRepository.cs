using Database.Models.Order;

namespace Database.Interfaces;

public interface IOrderPaymentTypeRepository
{
    Task<IEnumerable<OrderPaymentTypeModel>> GetAllAsync();
}