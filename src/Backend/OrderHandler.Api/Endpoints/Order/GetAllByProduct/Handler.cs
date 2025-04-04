using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Order.GetAllByProduct;

public class Handler(IOrderRepository repo) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/orders/products/{productId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var orders = await repo.GetAllOrdersWithProductAsync(req.ProductId);

        await SendAsync(new Response
        {
            OrdersWithProduct = orders

        }, cancellation:ct);
    }
}