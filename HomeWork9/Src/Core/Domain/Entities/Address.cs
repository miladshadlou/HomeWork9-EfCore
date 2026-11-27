namespace HomeWork9.Domain.Entities;

public class Address
{
    public int Id { get; set; }

    public string Province { get; set; } = null!;

    public string City { get; set; } = null!;

    public string FullAddress { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string ReceiverName { get; set; } = null!;

    public string ReceiverPhone { get; set; } = null!;

    public bool IsDefault { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}
