using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {

        // تنظیم تعداد
        builder.Property(ci => ci.Quantity)
            .IsRequired();

        // Composite Unique Index
        builder.HasIndex(ci => new
        {
            ci.CartId,
            ci.BookId
        })
        .IsUnique();

        // محدودیت تعداد اقلام سبد خرید
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_CartItems_Quantity",
                "[Quantity] > 0");
        });

        // تنظیم رابطه CartItem و Cart
        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        // تنظیم رابطه CartItem و Book
        builder.HasOne(ci => ci.Book)
            .WithMany()
            .HasForeignKey(ci => ci.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}