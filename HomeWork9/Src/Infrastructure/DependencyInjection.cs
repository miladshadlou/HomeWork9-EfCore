using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HomeWork9.Infrastructure.Persistence;
using HomeWork9.Application.Interfaces;
namespace HomeWork9.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<BookStoreDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IBookStoreDbContext>(provider =>
            provider.GetRequiredService<BookStoreDbContext>());

        return services;

    }
}