using System.Net;

namespace HomeWork9.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public ICollection<Address> Addresses { get; set; }
        = new List<Address>();

    public Cart? Cart { get; set; }

    public ICollection<Order> Orders { get; set; }
        = new List<Order>();

}
