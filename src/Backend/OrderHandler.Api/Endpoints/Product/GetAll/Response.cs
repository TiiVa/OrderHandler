using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Api.Endpoints.Product.GetAll;

public class Response
{
    public IEnumerable<ProductDto> Products { get; set; }
}