using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderHandler.DataAccess.RepositoryInterfaces;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.Api.Endpoints.User.Add;

public class Handler(IUserRepository repo) : Endpoint<Request, Results<Ok, BadRequest>>
{
    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, BadRequest>> HandleAsync(Request req, CancellationToken ct)
    {
        
        var userToAdd = new UserDto()
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email,
            Phone = req.Phone,
            Role = req.Role,
            Password = req.Password,
            StreetAddress = req.StreetAddress,
            ZipCode = req.ZipCode,
            City = req.City,
            Country = req.Country,
            Orders = req.Orders,
            IsActive = req.IsActive
        };

        var allUsers = await repo.GetAllAsync();

        if (allUsers.Any(u => u.Email.Equals(req.Email)))
        {
            return TypedResults.BadRequest();
        }

        await repo.AddAsync(userToAdd);

        return TypedResults.Ok();
    }
}