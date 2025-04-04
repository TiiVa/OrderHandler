using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;

namespace OrderHandler.Api.Endpoints.Order.Add;

public class Handler(IOrderRepository repo) : Endpoint<Request, Results<Ok, NotFound>>
{
    public override void Configure()
    {
        Post("/orders");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound>> HandleAsync(Request req, CancellationToken ct)
    {
        var newOrder = new OrderDto()
        {
            OrderSum = req.OrderSum,
            IsActive = req.IsActive,
            OrderProducts = req.OrderProducts,
            UserId = req.UserId
        };

        if (req.UserId == 0)
        {
            return TypedResults.NotFound();
        }

        await repo.AddAsync(newOrder);

        return TypedResults.Ok();
    }
}