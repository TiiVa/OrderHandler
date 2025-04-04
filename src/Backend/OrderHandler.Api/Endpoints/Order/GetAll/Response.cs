
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Api.Endpoints.Order.GetAll;

public class Response
{
    public IEnumerable<OrderDto> Orders { get; set; }
}