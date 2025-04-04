using OrderHandler.DataTransferContracts.DTOs.OrderProduct;

namespace OrderHandler.Api.Endpoints.Product.Add;

public class Request
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
    public List<OrderProductDto> Orders { get; set; } = new();

}