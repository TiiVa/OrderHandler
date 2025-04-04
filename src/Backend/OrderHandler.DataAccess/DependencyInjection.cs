using Microsoft.Extensions.DependencyInjection;
using OrderHandler.DataAccess.Repositories;
using OrderHandler.DataAccess.RepositoryInterfaces;

namespace OrderHandler.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}