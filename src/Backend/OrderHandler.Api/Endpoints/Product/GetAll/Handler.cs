using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Product.GetAll;

public class Handler(IProductRepository repo) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var products = await repo.GetAllAsync();

        await SendAsync(new Response()
        {
            Products = products
        }, cancellation: ct);
    }
}