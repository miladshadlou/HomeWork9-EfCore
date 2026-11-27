using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HomeWork9.Application.Interfaces;
namespace HomeWork9.Infrastructure.Persistence;

public class BookStoreDbContext : DbContext, IBookStoreDbContext
{
    public BookStoreDbContext(
        DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    // User
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Address> Addresses { get; set; } = null!;

    // Book
    public DbSet<Book> Books { get; set; } = null!;

    public DbSet<Author> Authors { get; set; } = null!;

    public DbSet<Publisher> Publishers { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    // Many-to-Many
    public DbSet<BookAuthor> BookAuthors { get; set; } = null!;

    public DbSet<BookCategory> BookCategories { get; set; } = null!;

    // Cart
    public DbSet<Cart> Carts { get; set; } = null!;

    public DbSet<CartItem> CartItems { get; set; } = null!;

    // Order
    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BookStoreDbContext).Assembly);
    }
}