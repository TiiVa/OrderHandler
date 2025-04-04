using OrderHandler.DataTransferContracts.DTOs.OrderProduct;

namespace OrderHandler.Api.Endpoints.Order.Add;

public class Request
{
   
    public double OrderSum { get; set; }
    public List<OrderProductDto> OrderProducts { get; set; }
    public int UserId { get; set; }
    public bool IsActive { get; set; }
}