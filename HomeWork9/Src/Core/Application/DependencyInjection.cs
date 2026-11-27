using HomeWork9.Application.Interfaces;
using HomeWork9.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HomeWork9.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();

        return services;
    }
}