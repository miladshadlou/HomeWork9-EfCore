using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        // Primary Key
        builder.HasKey(a => a.Id);

        // تنظیمات نام استان
        builder.Property(a => a.Province)
            .IsRequired()
            .HasMaxLength(100);

        // تنظیمات نام شهر
        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);

        // تنظیمات کد پستی
        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        // تنظیمات آدرس کامل
        builder.Property(a => a.FullAddress)
            .IsRequired()
            .HasMaxLength(500);

        // تنظیمات نام گیرنده
        builder.Property(a => a.ReceiverName)
            .IsRequired()
            .HasMaxLength(150);

        // تنظیمات موبایل گیرنده
        builder.Property(a => a.ReceiverPhone)
            .IsRequired()
            .HasMaxLength(20);

        // تنظیمات آدرس پیش فرض؟
        builder.Property(a => a.IsDefault)
            .HasDefaultValue(false);

        // هر کاربر حداکثر یک آدرس پیش‌فرض داشته باشد
        builder.HasIndex(a => a.UserId)
            .IsUnique()
            .HasFilter("[IsDefault] = 1");


        // رابطه Address و User
        builder.HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}
