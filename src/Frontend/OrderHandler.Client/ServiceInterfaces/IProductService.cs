using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Client.ServiceInterfaces;

public interface IProductService : IService<ProductDto, int>
{
    Task<IEnumerable<ProductDto>> GetAllInactiveProducts();
}