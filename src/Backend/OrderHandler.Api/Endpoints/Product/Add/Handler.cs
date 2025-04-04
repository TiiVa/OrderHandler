using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.Api.Endpoints.Product.Add;

public class Handler(IProductRepository repo) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, BadRequest>> HandleAsync(Request req, CancellationToken ct)
    {
        var allProducts = await repo.GetAllAsync();

        if (allProducts.Any(p => p.Name == req.Name))
        {
            return TypedResults.BadRequest();
        }

        var productToAdd = new ProductDto()
        {
            Name = req.Name,
            Description = req.Description,
            Price = req.Price,
            Color = req.Color,
            IsActive = req.IsActive,
            ProductOrders = req.Orders

        };

        await repo.AddAsync(productToAdd);

        return TypedResults.Ok();

    }
}