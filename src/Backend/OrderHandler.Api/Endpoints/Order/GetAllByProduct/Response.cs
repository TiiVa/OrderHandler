using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Api.Endpoints.Order.GetAllByProduct;

public class Response
{
    public IEnumerable<OrderDto> OrdersWithProduct { get; set; } = [];
}