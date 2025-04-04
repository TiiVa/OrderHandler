using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.User.GetById;

public class Handler(IUserRepository repo) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/users/{id}");
        AllowAnonymous();
    }


    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await repo.GetByIdAsync(req.Id);

        await SendAsync(new Response()
        {
            User = user,
        }, cancellation: ct);

    }
}