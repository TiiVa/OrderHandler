using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.Order.Delete;

public class Handler(IOrderRepository repo) : Endpoint<Request, Results<Ok, NotFound>>
{
    public override void Configure()
    {
        Delete("/orders/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound>> HandleAsync(Request req, CancellationToken ct)
    {
        var orderToInactivate = await repo.GetByIdAsync(req.Id);

        if (orderToInactivate is null)
        {
            return TypedResults.NotFound();
        }

        await repo.DeleteAsync(orderToInactivate.Id);

        return TypedResults.Ok();
    }
}