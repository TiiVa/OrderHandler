using System.ComponentModel.DataAnnotations;
using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;

namespace OrderHandler.DataTransferContracts.DTOs.Product;

public class ProductDto : IEntity<int>
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
    public List<OrderProductDto>? ProductOrders { get; set; }

}