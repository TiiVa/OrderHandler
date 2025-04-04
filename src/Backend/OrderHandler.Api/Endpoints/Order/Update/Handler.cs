using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Order.Update;

public class Handler(IOrderRepository repo) : Endpoint<Request, Results<Ok, BadRequest, NotFound>>
{
    public override void Configure()
    {
        Put("/orders/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, BadRequest, NotFound>> HandleAsync(Request req, CancellationToken ct)
    {
        var orderToUpdate = await repo.GetByIdAsync(req.Id);

        if (orderToUpdate is null)
        {
            return TypedResults.NotFound();
        }

        orderToUpdate.OrderSum = req.OrderSum;
        orderToUpdate.IsActive = req.IsActive;

        if (req.IsActive || req.OrderSum == null)
        {
            return TypedResults.BadRequest();
        }

        await repo.UpdateAsync(orderToUpdate, req.Id);

        return TypedResults.Ok();

    }
}