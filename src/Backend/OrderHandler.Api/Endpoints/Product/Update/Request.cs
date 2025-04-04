using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Api.Endpoints.Product.Update;

public class Request
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
    public List<OrderDto> Orders { get; set; } = new();
}