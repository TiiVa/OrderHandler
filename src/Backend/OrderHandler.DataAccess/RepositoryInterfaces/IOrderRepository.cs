using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.DataAccess.RepositoryInterfaces;

public interface IOrderRepository : IRepository<OrderDto, int>
{
    Task<IEnumerable<OrderDto>> GetAllOrdersWithProductAsync(int productId);
    Task<IEnumerable<OrderDto>> GetAllOrdersForAUserAsync(int userId);
}