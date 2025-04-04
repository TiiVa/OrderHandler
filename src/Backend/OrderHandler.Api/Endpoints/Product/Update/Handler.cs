using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Product.Update;

public class Handler(IProductRepository repo) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Put("/products/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, BadRequest>> HandleAsync(Request req, CancellationToken ct)
    {
        var productToUpdate = await repo.GetByIdAsync(req.Id);

        if (productToUpdate.Id == 0)
        {
            return TypedResults.BadRequest();
        }

        productToUpdate.Name = req.Name;
        productToUpdate.Description = req.Description;
        productToUpdate.Price = req.Price;
        productToUpdate.Color = req.Color;
        productToUpdate.IsActive = req.IsActive;

        await repo.UpdateAsync(productToUpdate, req.Id);

        return TypedResults.Ok();
    }
}