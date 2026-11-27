using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary Key
        builder.HasKey(u => u.Id);

        // تنظیمات نام کاربر
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        // تنظیمات ایمیل کاربر
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        // تنظیمات رمز عبور کاربر
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

    }

}
