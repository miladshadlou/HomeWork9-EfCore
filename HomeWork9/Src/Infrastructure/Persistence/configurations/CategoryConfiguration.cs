using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id); // الزامی نیست قابل تشخیص توسط EFCore

        // تنظیمات نام دسته بندی
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // تنظیمات شرح دسته بندی
        builder.Property(c => c.Description)
            .HasMaxLength(500);

        // تنظیم Backing Field
        builder.Navigation(c => c.BookCategories)
            .HasField("_bookCategories")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}