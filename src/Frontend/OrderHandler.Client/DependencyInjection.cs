using Microsoft.AspNetCore.Authentication.Cookies;
using OrderHandler.CommonInterfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using OrderHandler.Client.ServiceInterfaces;
using OrderHandler.Client.Services;

namespace OrderHandler.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddSingleton<CartService>();

        services.AddAuth();

        return services;


    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "cookie_token";
                options.LoginPath = "/login";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
                options.AccessDeniedPath = "/access-denied";

            });
        services.AddAuthorization();
        services.AddCascadingAuthenticationState();

        return services;
    }
}