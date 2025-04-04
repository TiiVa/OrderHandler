using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Api.Endpoints.Product.GetById;

public class Response
{
    public ProductDto Product { get; set; }
}