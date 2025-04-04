

using Microsoft.EntityFrameworkCore;
using OrderHandler.DataAccess.Entities;
using OrderHandler.DataAccess.RepositoryInterfaces;
using OrderHandler.DataTransferContracts.DTOs.Order;
using OrderHandler.DataTransferContracts.DTOs.OrderProduct;
using OrderHandler.DataTransferContracts.DTOs.Product;

namespace OrderHandler.DataAccess.Repositories;

public class ProductRepository(OrderHandlerDbContext context) : IProductRepository
{
  
   
    public async Task<IEnumerable<ProductDto>> GetAllAsync() 
    {
        var products = await context.Products
            .Where(p => p.IsActive == true)
            .Include(p => p.ProductOrders)
            .ThenInclude(orderProduct => orderProduct.Order)
            .ThenInclude(order => order.User)
            .Include(product => product.ProductOrders)
            .ThenInclude(orderProduct => orderProduct.Order)
            .ThenInclude(order => order.OrderProducts)
            .ThenInclude(orderProduct => orderProduct.Product)
            .ToListAsync();


        var productsToReturn = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Color = p.Color,
            IsActive = p.IsActive,
            ProductOrders = p.ProductOrders.Select(o => new OrderProductDto
            {
                OrderId = o.OrderId,
                Name = o.Product.Name,
                Price = o.Product.Price,
                Description = o.Product.Description,
                Order = o.Order != null ? new OrderDto
                {
                    Id = o.Order.Id,
                    OrderSum = o.Order.OrderSum,
                    UserId = o.Order.User.Id,
                    IsActive = o.Order.IsActive,
                    OrderProducts = o.Order.OrderProducts.Select(o => new OrderProductDto
                    {
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Name = o.Product.Name,
                        Price = o.Product.Price,
                        Description = o.Product.Description,
                        Quantity = o.Quantity,
                        Order = o.Order != null ? new OrderDto
                        {
                            Id = o.OrderId,
                            UserId = o.Order.User.Id,
                            IsActive = o.Order.IsActive
                            
                        } : null
                    }).ToList()

                } : null,
                ProductId = o.ProductId,
                Quantity = o.Quantity
            }).ToList()
        });

        return productsToReturn;

    }

    public async Task<IEnumerable<ProductDto>> GetAllInactiveAsync()
    {
        var products = await context.Products
            .Where(p => p.IsActive == false)
            .Include(p => p.ProductOrders)
            .ThenInclude(orderProduct => orderProduct.Order)
            .ThenInclude(order => order.User)
            .Include(product => product.ProductOrders)
            .ThenInclude(orderProduct => orderProduct.Order)
            .ThenInclude(order => order.OrderProducts)
            .ThenInclude(orderProduct => orderProduct.Product)
            .ToListAsync();


        var productsToReturn = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Color = p.Color,
            IsActive = p.IsActive,
            ProductOrders = p.ProductOrders.Select(o => new OrderProductDto
            {
                OrderId = o.OrderId,
                Name = o.Product.Name,
                Price = o.Product.Price,
                Description = o.Product.Description,
                Order = o.Order != null ? new OrderDto
                {
                    Id = o.Order.Id,
                    OrderSum = o.Order.OrderSum,
                    UserId = o.Order.User.Id,
                    IsActive = o.Order.IsActive,
                    OrderProducts = o.Order.OrderProducts.Select(o => new OrderProductDto
                    {
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Name = o.Product.Name,
                        Price = o.Product.Price,
                        Description = o.Product.Description,
                        Quantity = o.Quantity,
                        Order = o.Order != null ? new OrderDto
                        {
                            Id = o.OrderId,
                            UserId = o.Order.User.Id,
                            IsActive = o.Order.IsActive

                        } : null
                    }).ToList()

                } : null,
                ProductId = o.ProductId,
                Quantity = o.Quantity
            }).ToList()
        });

        return productsToReturn;

    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await context.Products
            .Where(p => p.IsActive == true)
            .Include(p => p.ProductOrders)
            .ThenInclude(orderProduct => orderProduct.Order)
            .ThenInclude(order => order.User)
            .Include(product => product.ProductOrders)
            .ThenInclude(orderProduct => orderProduct.Order)
            .ThenInclude(order => order.OrderProducts)
            .ThenInclude(orderProduct => orderProduct.Product)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product is null)
        {
            return new ProductDto();
        }

        var productToReturn = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Color = product.Color ?? null,
            IsActive = product.IsActive,
            ProductOrders = product.ProductOrders.Select(o => new OrderProductDto
            {
                OrderId = o.OrderId,
                Name = o.Product.Name,
                Price = o.Product.Price,
                Description = o.Product.Description,
                Order = o.Order != null ? new OrderDto
                {
                    Id = o.Order.Id,
                    OrderSum = o.Order.OrderSum,
                    UserId = o.Order.User.Id,
                    IsActive = o.Order.IsActive,
                    OrderProducts = o.Order.OrderProducts.Select(o => new OrderProductDto
                    {
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Name = o.Product.Name,
                        Price = o.Product.Price,
                        Description = o.Product.Description,
                        Quantity = o.Quantity,
                        Order = o.Order != null ? new OrderDto
                        {
                            Id = o.OrderId,
                            UserId = o.Order.User.Id,
                            IsActive = o.Order.IsActive,

                        } : null
                    }).ToList()

                } : null,
                ProductId = o.ProductId,
                Quantity = o.Quantity
            }).ToList()
        };



        return productToReturn;
    }

    public async Task AddAsync(ProductDto entity)
    {
        var productToAdd = new Product()
        {
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Color = "Green",
            IsActive = true

        };

        await context.AddAsync(productToAdd);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductDto entity, int id)
    {
        var productToUpdate = await context.Products.FindAsync(id);

        if (productToUpdate is null)
        {
            return;
        }

        productToUpdate.Name = entity.Name;
        productToUpdate.Description = entity.Description;
        productToUpdate.Price = entity.Price;
        productToUpdate.Color = entity.Color;
        productToUpdate.IsActive = entity.IsActive;

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var productToInactivate = await context.Products.FindAsync(id);

        if (productToInactivate is null)
        {
            return;
        }

        var entityEntry = context.Products.Update(productToInactivate);
        entityEntry.Property(p => p.IsActive).CurrentValue = false;

        await context.SaveChangesAsync();
    }
}