using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.User.Delete;

public class Handler(IUserRepository repo) : Endpoint<Request, Results<Ok, NotFound>>
{
    public override void Configure()
    {
        Delete("/users/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound>> HandleAsync(Request req, CancellationToken ct)
    {
        var userToInactivate = await repo.GetByIdAsync(req.Id);

        if (userToInactivate.Id == 0)
        {
            return TypedResults.NotFound();
        }

        await repo.DeleteAsync(userToInactivate.Id);

        return TypedResults.Ok();
    }
}