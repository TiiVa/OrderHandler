using Microsoft.EntityFrameworkCore;
using OrderHandler.DataAccess.Entities;
using OrderHandler.DataAccess.RepositoryInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;
using OrderHandler.DataTransferContracts.DTOs.User;

namespace OrderHandler.DataAccess.Repositories;

public class UserRepository(OrderHandlerDbContext context) : IUserRepository
{
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await context.Users
            .Where(u => u.IsActive == true)
            .Include(u => u.Orders)
            .ThenInclude(order => order.OrderProducts)
            .ToListAsync();


        var usersToReturn = users.Select(u => new UserDto()
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Phone = u.Phone,
            Password = u.Password,
            Role = u.Role,
            StreetAddress = u.StreetAddress,
            ZipCode = u.ZipCode,
            City = u.City,
            Country = u.Country,
            IsActive = u.IsActive,
            Orders = u.Orders.Select(o => new OrderDto()
            {
                Id = o.Id, 
                OrderSum = o.OrderSum,
                IsActive = o.IsActive,
                OrderProducts = o.OrderProducts.Select(p => new OrderProductDto()
                {
                    ProductId = p.ProductId,
                    OrderId = p.OrderId,
                    Quantity = p.Quantity,
                    Order = p.Order != null ? new OrderDto()
                    {
                        Id = p.OrderId,
                        UserId = p.Order.User.Id
                    } : null

                }).ToList()
            }).ToList()
        });

        return usersToReturn;

    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await context.Users
            .Where(u => u.IsActive == true)
            .Include(u => u.Orders)
            .ThenInclude(order => order.OrderProducts)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            return new UserDto();
        }

        var userToReturn = new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            Password = user.Password,
            Role = user.Role,
            StreetAddress = user.StreetAddress,
            ZipCode = user.ZipCode,
            City = user.City,
            Country = user.Country,
            IsActive = user.IsActive,
            Orders = user.Orders.Select(o => new OrderDto()
            {
                Id = o.Id,
                OrderSum = o.OrderSum,
                IsActive = o.IsActive,
                OrderProducts = o.OrderProducts.Select(p => new OrderProductDto()
                {
                   OrderId = p.OrderId,
                   ProductId = p.ProductId,
                   Quantity = p.Quantity,
                   Order = p.Order != null ? new OrderDto()
                   {
                       Id = p.OrderId,
                       UserId = p.Order.User.Id
                   } : null

                }).ToList()
            }).ToList()
        };

        return userToReturn;
    }

    public async Task AddAsync(UserDto entity)
    {
        if (entity.Phone == "")
        {
            entity.Phone = "000";
        }

        var newUser = new User()
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.Phone,
            Password = entity.Password,
            Role = "User",
            StreetAddress = entity.StreetAddress,
            ZipCode = entity.ZipCode,
            City = entity.City,
            Country = entity.Country,
            IsActive = true,
            Orders = new List<Order>()
        };

        

        await context.AddAsync(newUser);
        await context.SaveChangesAsync();
        
    }

    public async Task UpdateAsync(UserDto entity, int id)
    {
        var userToUpdate = await context.Users.FindAsync(id);

        if (userToUpdate is null)
        {
            return;
        }

        userToUpdate.FirstName = entity.FirstName;
        userToUpdate.LastName = entity.LastName;
        userToUpdate.Email = entity.Email;
        userToUpdate.Phone = entity.Phone;
        userToUpdate.Password = entity.Password;
        userToUpdate.Role = entity.Role;
        userToUpdate.StreetAddress = entity.StreetAddress;
        userToUpdate.ZipCode = entity.ZipCode;
        userToUpdate.City = entity.City;
        userToUpdate.Country = entity.Country;
        userToUpdate.IsActive = entity.IsActive;

        await context.SaveChangesAsync();

    }

    public async Task DeleteAsync(int id)
    {
        var userToInactivate = await context.Users.FindAsync(id);

        if (userToInactivate is null)
        {
            return;
        }

        var entityEntry = context.Users.Update(userToInactivate);
        entityEntry.Property(u => u.IsActive).CurrentValue = false;

        await context.SaveChangesAsync();
    }
}