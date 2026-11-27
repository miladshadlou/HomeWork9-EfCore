using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeWork9.Infrastructure.Persistence.configurations;

public class OrderItemConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // Primary Key
        builder.HasKey(oi => oi.Id);

        // Quantity
        builder.Property(oi => oi.Quantity)
            .IsRequired();

        // UnitPrice
        builder.Property(oi => oi.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        // TotalPrice
        builder.Property(oi => oi.TotalPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        // Check Constraints
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_OrderItems_Quantity",
                "[Quantity] > 0");

            t.HasCheckConstraint(
                "CK_OrderItems_UnitPrice",
                "[UnitPrice] >= 0");

            t.HasCheckConstraint(
                "CK_OrderItems_TotalPrice",
                "[TotalPrice] >= 0");
        });

        // Composite Unique Index
        builder.HasIndex(oi => new
        {
            oi.OrderId,
            oi.BookId
        }).IsUnique();

        // رابطه OrderItem و Order
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // رابطه OrderItem و Book
        builder.HasOne(oi => oi.Book)
            .WithMany()
            .HasForeignKey(oi => oi.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
