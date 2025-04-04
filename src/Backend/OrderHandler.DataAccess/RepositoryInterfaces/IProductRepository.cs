using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.DataAccess.RepositoryInterfaces;

public interface IProductRepository : IRepository<ProductDto, int>
{
    Task<IEnumerable<ProductDto>> GetAllInactiveAsync();
}