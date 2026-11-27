using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HomeWork9.Infrastructure.Persistence.configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // Primary Key
        builder.HasKey(b => b.Id);

        // عنوان کتاب
        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        // ISBN
        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(b => b.ISBN)
            .IsUnique();

        // شرح کتاب
        builder.Property(b => b.Description)
            .HasMaxLength(2000);

        // وضعیت فعال بودن کتاب
        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // قیمت کتاب
        builder.Property(b => b.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        // موجودی کتاب
        builder.Property(b => b.Stock)
            .IsRequired();

        // constraint on price & stock
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Books_Price",
                "[Price] >= 0");

            t.HasCheckConstraint(
                "CK_Books_Stock",
                "[Stock] >= 0");
        });

        // تصویر جلد کتاب
        builder.Property(b => b.CoverImageUrl)
            .HasMaxLength(500);

        // BookAuthors navigation configuration
        builder.Navigation(b => b.BookAuthors)
            .HasField("_bookAuthors")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // BookCategories navigation configuration
        builder.Navigation(b => b.BookCategories)
            .HasField("_bookCategories")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
