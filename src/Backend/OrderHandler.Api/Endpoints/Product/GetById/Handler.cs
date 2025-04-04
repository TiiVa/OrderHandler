using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Product.GetById;

public class Handler(IProductRepository repo) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/products/{id}");
        AllowAnonymous();
        
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(req.Id);

        await SendAsync(new Response()
        {
            Product = product
        }, cancellation: ct);
    }
}