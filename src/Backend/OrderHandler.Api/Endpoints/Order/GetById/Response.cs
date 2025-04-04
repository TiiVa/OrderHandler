using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Api.Endpoints.Order.GetById;

public class Response
{
    public OrderDto Order { get; set; }
}