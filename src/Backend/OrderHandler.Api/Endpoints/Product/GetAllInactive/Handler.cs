using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Product.GetAllInactive;

public class Handler(IProductRepository repo) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
       Get("/products/status");
       AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var inactiveProducts = await repo.GetAllInactiveAsync();

        await SendAsync(new Response()
        {
            InactiveProducts = inactiveProducts
        }, cancellation: ct);
    }
}