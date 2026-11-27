namespace HomeWork9.Domain.Entities;

public class CartItem
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int CartId { get; set; }

    public Cart Cart { get; set; } = null!;

    public int BookId { get; set; }

    public Book Book { get; set; } = null!;
}
