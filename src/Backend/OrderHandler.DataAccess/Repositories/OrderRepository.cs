using Microsoft.EntityFrameworkCore;
using OrderHandler.DataAccess.Entities;
using OrderHandler.DataAccess.RepositoryInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.DataAccess.Repositories;

public class OrderRepository(OrderHandlerDbContext context) : IOrderRepository
{
    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await context.Orders
            .Where(o => o.IsActive == true)
            .Include(o => o.OrderProducts).ThenInclude(orderProduct => orderProduct.Product)
            .Include(o => o.User)
            .ToListAsync();

        var ordersToReturn = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderSum = o.OrderSum,
            IsActive = o.IsActive,
            UserId = o.User.Id,
            OrderProducts = o.OrderProducts.Select(p => new OrderProductDto
                {
                    ProductId = p.ProductId,
                    OrderId = p.OrderId,
                    Quantity = p.Quantity,
                    Price = p.Product.Price,
                    Name = p.Product.Name,
                    Color = p.Product.Color,
                    Description = p.Product.Description,
                    Order = p.Order != null ? new OrderDto
                    {
                        Id = p.OrderId,
                        UserId = p.Order.User.Id
                    } : null
                }).ToList() 

        });

        return ordersToReturn;
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersWithProductAsync(int productId)
    {
        var orders = context.Orders
            .Where(o => o.OrderProducts.Any(p => p.ProductId == productId) && o.IsActive == true)
            .Include(o => o.OrderProducts).ThenInclude(orderProduct => orderProduct.Product)
            .Include(o => o.User)
            .ToList();

        var ordersToReturn = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderSum = o.OrderSum,
            IsActive = o.IsActive,
            UserId = o.User.Id,
            OrderProducts = o.OrderProducts.Select(p => new OrderProductDto
            {
                ProductId = p.ProductId,
                OrderId = p.OrderId,
                Quantity = p.Quantity,
                Price = p.Product.Price,
                Name = p.Product.Name,
                Color = p.Product.Color,
                Description = p.Product.Description,
                Order = p.Order != null ? new OrderDto
                {
                    Id = p.OrderId,
                    UserId = p.Order.User.Id
                } : null,
                Product = p.Product != null ? new ProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name
                } : null
            }).ToList()

        });

        return ordersToReturn;
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersForAUserAsync(int userId)
    {
        var orders = context.Orders
            .Where(o => o.User.Id == userId)
            .Include(o => o.OrderProducts).ThenInclude(orderProduct => orderProduct.Product)
            .Include(o => o.User)
            .ToList();

        var ordersToReturn = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderSum = o.OrderSum,
            IsActive = o.IsActive,
            UserId = o.User.Id,
            OrderProducts = o.OrderProducts.Select(p => new OrderProductDto
            {
                ProductId = p.ProductId,
                OrderId = p.OrderId,
                Quantity = p.Quantity,
                Price = p.Product.Price,
                Name = p.Product.Name,
                Color = p.Product.Color,
                Description = p.Product.Description,
                Order = p.Order != null ? new OrderDto
                {
                    Id = p.OrderId,
                    UserId = p.Order.User.Id
                } : null,
                Product = p.Product != null ? new ProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name
                } : null
            }).ToList()

        });

        return ordersToReturn;
    }

    public async Task<OrderDto> GetByIdAsync(int id)
    {
        var order = await context.Orders
            .Where(o => o.IsActive == true)
            .Include(o => o.OrderProducts).ThenInclude(orderProduct => orderProduct.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
        {
            return new OrderDto();
        }

        var orderToReturn = new OrderDto()
        {
            Id = order.Id,
            OrderSum = order.OrderSum,
            IsActive = order.IsActive,
            UserId = order.User.Id,
            OrderProducts = order.OrderProducts.Select(p => new OrderProductDto
            {
                ProductId = p.ProductId,
                OrderId = p.OrderId,
                Quantity = p.Quantity,
                Price = p.Product.Price,
                Name = p.Product.Name,
                Color = p.Product.Color,
                Order = new OrderDto
                {
                    Id = p.OrderId,
                    UserId = p.Order.User.Id
                }
            }).ToList()
        };

        return orderToReturn;


    }

    public async Task AddAsync(OrderDto entity)
    {
        var userForOrder = await context.Users.FindAsync(entity.UserId);

        if (userForOrder is null)
        {
            return;
        }

        var productsForOrder = new List<OrderProduct>();

        foreach (var productDto in entity.OrderProducts)
        {
            var productFromDatabase = await context.Products.FindAsync(productDto.ProductId);

            var existingOrderProduct = productsForOrder // Does the current item already exist in the products list for the order?
                .FirstOrDefault(p => p.ProductId == productDto.ProductId);

            if (existingOrderProduct != null) // If the current item already exists in the product list for the order 
            {
                existingOrderProduct.Quantity += productDto.Quantity; // update the quantity
            }
            else // otherwise add the item to the list 
            {
                var orderProduct = new OrderProduct()
                {
                    ProductId = productDto.ProductId,
                    Quantity = productDto.Quantity,
                    Product = productFromDatabase

                };

                productsForOrder.Add(orderProduct);
            }

        }

        var newOrder = new Order()
        {
            OrderSum = entity.OrderSum,
            IsActive = true,
            OrderProducts = productsForOrder,
            User = userForOrder
        };

        

        await context.AddAsync(newOrder);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(OrderDto entity, int id)
    {
        var orderToUpdate = await context.Orders.FindAsync(id);

        if (orderToUpdate is null)
        {
            return;
        }

        orderToUpdate.OrderSum = entity.OrderSum;
        orderToUpdate.IsActive = entity.IsActive;

        await context.SaveChangesAsync();

    }

    public async Task DeleteAsync(int id)
    {
        var orderToInactivate = await context.Orders.FindAsync(id);

        if (orderToInactivate is null)
        {
            return;
        }

        var entityEntry = context.Orders.Update(orderToInactivate);
        entityEntry.Property(o => o.IsActive).CurrentValue = false;

        await context.SaveChangesAsync();
    }
}