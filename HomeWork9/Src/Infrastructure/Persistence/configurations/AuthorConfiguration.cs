using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        // تنظیمات نام نویسنده
        builder.Property(a => a.FullName)
            .IsRequired()
            .HasMaxLength(100);

        // تنظیمات بیوگرافی نویسنده
        builder.Property(a => a.Biography)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Navigation(a => a.BookAuthors)
            .HasField("_bookAuthors")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
