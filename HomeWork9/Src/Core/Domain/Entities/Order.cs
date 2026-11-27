using HomeWork9.Domain.Enums;
namespace HomeWork9.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
    public string ReceiverName { get; set; } = null!;

    public string ReceiverPhone { get; set; } = null!;

    public string ShippingProvince { get; set; } = null!;

    public string ShippingCity { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public string ShippingPostalCode { get; set; } = null!;
}
