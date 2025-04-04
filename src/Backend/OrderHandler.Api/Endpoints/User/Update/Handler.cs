using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.Api.Endpoints.User.Update;

public class Handler(IUserRepository repo) : Endpoint<Request, Results<Ok, NotFound>>
{
    public override void Configure()
    {
        Put("/users/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, NotFound>> HandleAsync(Request req, CancellationToken ct)
    {
        var userToUpdate = await repo.GetByIdAsync(req.Id);

        if (userToUpdate is null)
        {
            return TypedResults.NotFound();
        }

        userToUpdate.FirstName = req.FirstName;
        userToUpdate.LastName = req.LastName;
        userToUpdate.Email = req.Email;
        userToUpdate.Phone = req.Phone;
        userToUpdate.Password = req.Password;
        userToUpdate.Role = req.Role;
        userToUpdate.StreetAddress = req.StreetAddress;
        userToUpdate.ZipCode = req.ZipCode;
        userToUpdate.City = req.City;
        userToUpdate.Country = req.Country;
        userToUpdate.IsActive = req.IsActive;


        await repo.UpdateAsync(userToUpdate, req.Id);

        return TypedResults.Ok();
    }
}