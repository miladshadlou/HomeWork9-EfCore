using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeWork9.Application.Interfaces;

public interface IBookStoreDbContext
{
    DbSet<Book> Books { get; }

    DbSet<Publisher> Publishers { get; }
    
    DbSet<Author> Authors { get; }

    DbSet<Category> Categories { get; }

    DbSet<CartItem> CartItems { get; }

    DbSet<OrderItem> OrderItems { get; } 


    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}