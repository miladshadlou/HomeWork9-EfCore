using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HomeWork9.Infrastructure.Persistence.configurations;

public class PublisherConfiguration: IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        // تنظیمات نام ناشر
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // تنظیم آدرس وبسایت ناشر
        builder.Property(p => p.Website)
            .IsRequired()
            .HasMaxLength(100);

        // تنظیم رابطه Publisher و Book
        builder.HasMany(p => p.Books)
            .WithOne(b => b.Publisher)
            .HasForeignKey(b => b.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}