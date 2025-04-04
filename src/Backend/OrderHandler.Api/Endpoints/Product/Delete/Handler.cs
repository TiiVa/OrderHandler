using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Product.Delete;

public class Handler(IProductRepository repo) : Endpoint<Request, Results<Ok, NotFound>>
{
    public override void Configure()
    {
        Delete("/products/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound>> HandleAsync(Request req, CancellationToken ct)
    {
        var productToRemove = await repo.GetByIdAsync(req.Id);

        if (productToRemove.Id == 0)
        {
            return TypedResults.NotFound();
        }

        await repo.DeleteAsync(req.Id);

        return TypedResults.Ok();

    }
}