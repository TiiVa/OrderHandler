using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Api.Endpoints.Order.GetAllForAUser;

public class Response
{
    public IEnumerable<OrderDto> OrdersForAUser {get; set; }
}