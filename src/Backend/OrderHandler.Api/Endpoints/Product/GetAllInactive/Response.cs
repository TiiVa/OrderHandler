using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Api.Endpoints.Product.GetAllInactive;

public class Response
{
    public IEnumerable<ProductDto> InactiveProducts { get; set; }
}