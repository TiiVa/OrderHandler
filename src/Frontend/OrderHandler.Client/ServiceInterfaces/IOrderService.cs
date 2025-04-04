using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Client.ServiceInterfaces;

public interface IOrderService : IService<OrderDto, int>
{
    Task<IEnumerable<OrderDto>> GetAllOrdersWithProductAsync(int productId);
    Task<IEnumerable<OrderDto>> GetAllOrdersForAUserAsync(int userId);
}