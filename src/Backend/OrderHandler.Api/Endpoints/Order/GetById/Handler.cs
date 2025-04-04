using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Order.GetById;

public class Handler(IOrderRepository repo) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/orders/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var order = await repo.GetByIdAsync(req.Id);

        await SendAsync(new Response()
        {
            Order = order
        }, cancellation: ct);
    }
}