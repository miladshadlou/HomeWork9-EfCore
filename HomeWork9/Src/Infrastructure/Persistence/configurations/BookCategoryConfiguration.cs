using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.Configurations;

public class BookCategoryConfiguration
    : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        // Composite Primary Key
        builder.HasKey(bc => new
        {
            bc.BookId,
            bc.CategoryId
        });

        // رابطه BookCategory و Book
        builder.HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId);

        // رابطه BookCategory و Category
        builder.HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId);
    }
}
