using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Primary Key
        builder.HasKey(o => o.Id);

        // تنظیمات تاریخ ایجاد
        builder.Property(o => o.CreatedAt)
            .IsRequired();

        // تنظیمات مقدار کل
        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        // تنظیمات وضعیت
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // اطلاعات گیرنده
        builder.Property(o => o.ReceiverName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(o => o.ReceiverPhone)
            .IsRequired()
            .HasMaxLength(20);

        // اطلاعات مقصد ارسال
        builder.Property(o => o.ShippingProvince)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.ShippingCity)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.ShippingAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(o => o.ShippingPostalCode)
            .IsRequired()
            .HasMaxLength(20);


        // رابطه بین User و Order
        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
