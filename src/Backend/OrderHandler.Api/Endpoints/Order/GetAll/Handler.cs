using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Order.GetAll;

public class Handler(IOrderRepository repo) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var allOrders = await repo.GetAllAsync();

        await SendAsync(new Response()
        {
            Orders = allOrders
        }, cancellation: ct);
    }
}