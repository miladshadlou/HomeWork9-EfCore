using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.Configurations;

public class BookAuthorConfiguration
    : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        // Composite Primary Key
        builder.HasKey(ba => new
        {
            ba.BookId,
            ba.AuthorId
        });

        // رابطه BookAuthor و Book
        builder.HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);

        // رابطه BookAuthor و Author
        builder.HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);
    }
}