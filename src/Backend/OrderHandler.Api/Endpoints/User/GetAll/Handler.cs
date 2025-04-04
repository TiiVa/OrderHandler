using FastEndpoints;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.User.GetAll;

public class Handler(IUserRepository repo) : Endpoint<EmptyRequest, Response>
{
    public override void Configure()
    {
        Get("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var allUsers = await repo.GetAllAsync();

        await SendAsync(new Response()
        {
            Users = allUsers
        }, cancellation: ct);
    }
}