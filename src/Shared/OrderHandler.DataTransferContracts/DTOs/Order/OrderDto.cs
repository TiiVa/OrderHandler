using System.ComponentModel.DataAnnotations;
using OrderHandler.CommonInterfaces;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;

namespace OrderHandler.DataTransferContracts.DTOs.Order;

public class OrderDto : IEntity<int>
{
    public int Id { get; set; }
    public double OrderSum { get; set; }
    public List<OrderProductDto> OrderProducts { get; set; } = new();
    [Required]
    public int UserId { get; set; }
    public bool IsActive { get; set; }
}