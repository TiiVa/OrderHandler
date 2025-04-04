using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Order.GetAllForAUser;

public class Handler(IOrderRepository repo) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/orders/users/{userId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var ordersForAUser = await repo.GetAllOrdersForAUserAsync(req.UserId);

        await SendAsync(new Response()
        {
            OrdersForAUser = ordersForAUser
        }, cancellation: ct);
    }
}