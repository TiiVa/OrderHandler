using OrderHandler.DataTransferContracts.DTOs.Order;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.DataTransferContracts.DTOs.OrderProduct;

public class OrderProductDto
{
    public int OrderId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public OrderDto? Order { get; set; } 
    public int ProductId { get; set; }
    public ProductDto Product { get; set; } 
    public int Quantity { get; set; }
}